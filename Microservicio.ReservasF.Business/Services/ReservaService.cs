using Microservicio.ReservasF.Business.DTOs.Boleto;
using Microservicio.ReservasF.Business.DTOs.Equipaje;
using Microservicio.ReservasF.Business.DTOs.Factura;
using Microservicio.ReservasF.Business.DTOs.Reserva;
using Microservicio.ReservasF.Business.DTOs.ReservaDetalle;
using Microservicio.ReservasF.Business.Exceptions;
using Microservicio.ReservasF.Business.Integrations.Interfaces;
using Microservicio.ReservasF.Business.Interfaces;
using Microservicio.ReservasF.Business.Mappers;
using Microservicio.ReservasF.Business.Validators;
using Microservicio.ReservasF.DataManagement.Common;
using Microservicio.ReservasF.DataManagement.Interfaces;
using Microservicio.ReservasF.DataManagement.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microservicio.ReservasF.Business.Services;

public class ReservaService : IReservaService
{
    private static readonly string[] EstadosActivosReserva = ["PEN", "CON", "EMI"];

    private readonly IReservaDataService _reservaDataService;
    private readonly IFacturaDataService _facturaDataService;
    private readonly IBoletoService _boletoService;
    private readonly IEquipajeService _equipajeService;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IClienteIntegrationService _clienteIntegrationService;
    private readonly IVueloIntegrationService _vueloIntegrationService;

    private readonly ReservaValidator _validator;

    public ReservaService(
        IReservaDataService reservaDataService,
        IFacturaDataService facturaDataService,
        IBoletoService boletoService,
        IEquipajeService equipajeService,
        IUnitOfWork unitOfWork,
        IClienteIntegrationService clienteIntegrationService,
        IVueloIntegrationService vueloIntegrationService)
    {
        _reservaDataService = reservaDataService;
        _facturaDataService = facturaDataService;
        _boletoService = boletoService;
        _equipajeService = equipajeService;
        _unitOfWork = unitOfWork;
        _clienteIntegrationService = clienteIntegrationService;
        _vueloIntegrationService = vueloIntegrationService;
        _validator = new ReservaValidator();
    }

    public async Task<DataPagedResult<ReservaResponseDto>> GetPagedAsync(ReservaFilterDto filter)
    {
        _validator.ValidateFilter(filter);

        var filtro = ReservaBusinessMapper.ToFiltroDataModel(filter);
        var result = await _reservaDataService.GetPagedAsync(filtro);

        return new DataPagedResult<ReservaResponseDto>
        {
            Items = ReservaBusinessMapper.ToResponseDtoList(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<ReservaResponseDto?> GetByIdAsync(
        int idReserva,
        int? idClienteDelToken,
        string rolDelToken)
    {
        if (idReserva <= 0)
            throw new ValidationException("El id de la reserva debe ser mayor que 0.");

        var data = await _reservaDataService.GetByIdAsync(idReserva);

        if (data == null)
            return null;

        if (rolDelToken == "CLIENTE" &&
            (idClienteDelToken == null || data.IdCliente != idClienteDelToken))
        {
            throw new UnauthorizedBusinessException("No tienes permiso para ver esta reserva.");
        }

        return ReservaBusinessMapper.ToResponseDto(data);
    }

    public async Task<ReservaResponseDto> CreateAsync(
        ReservaRequestDto request,
        string creadoPorUsuario)
    {
        if (string.IsNullOrWhiteSpace(creadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario creador.");

        _validator.ValidateRequest(request);

        var clienteExiste = await _clienteIntegrationService.ExisteClienteAsync(request.IdCliente);
        if (!clienteExiste)
            throw new NotFoundException("El cliente indicado no existe.");

        var clienteActivo = await _clienteIntegrationService.ClienteActivoAsync(request.IdCliente);
        if (!clienteActivo)
            throw new BusinessException("El cliente indicado está inactivo o eliminado.");

        var vuelo = await _vueloIntegrationService.ObtenerVueloAsync(request.IdVuelo);
        if (vuelo == null)
            throw new NotFoundException("El vuelo indicado no existe.");

        if (vuelo.Estado != "ACTIVO" || vuelo.EstadoVuelo is not ("PROGRAMADO" or "DEMORADO"))
            throw new BusinessException("El vuelo no está disponible para nuevas reservas.");

        var detallesSolicitados = GetDetallesSolicitados(request);

        if (detallesSolicitados.Count == 0)
            throw new BusinessException("La reserva debe tener al menos un detalle.");

        foreach (var detalle in detallesSolicitados)
        {
            var pasajeroExiste = await _clienteIntegrationService.ExistePasajeroAsync(detalle.IdPasajero);
            if (!pasajeroExiste)
                throw new NotFoundException($"El pasajero {detalle.IdPasajero} no existe.");

            var pasajeroActivo = await _clienteIntegrationService.PasajeroActivoAsync(detalle.IdPasajero);
            if (!pasajeroActivo)
                throw new BusinessException($"El pasajero {detalle.IdPasajero} está inactivo o eliminado.");

            var pasajeroPertenece = await _clienteIntegrationService.PasajeroPerteneceAClienteAsync(
                detalle.IdPasajero,
                request.IdCliente);

            if (!pasajeroPertenece)
                throw new BusinessException($"El pasajero {detalle.IdPasajero} no pertenece al cliente indicado.");

            var asiento = await _vueloIntegrationService.ObtenerAsientoAsync(detalle.IdAsiento);
            if (asiento == null)
                throw new NotFoundException($"El asiento {detalle.IdAsiento} no existe.");

            if (asiento.Estado != "ACTIVO" || asiento.Eliminado)
                throw new BusinessException($"El asiento {detalle.IdAsiento} está inactivo o eliminado.");

            if (asiento.IdVuelo != request.IdVuelo)
                throw new BusinessException($"El asiento {detalle.IdAsiento} no pertenece al vuelo indicado.");

            if (!asiento.Disponible)
                throw new BusinessException($"El asiento {detalle.IdAsiento} no está disponible.");
        }

        EnsureNoDuplicadosEnRequest(detallesSolicitados);

        var existentes = await _reservaDataService.GetPagedAsync(new ReservaFiltroDataModel
        {
            IdVuelo = request.IdVuelo,
            PageNumber = 1,
            PageSize = 10000
        });

        foreach (var detalle in detallesSolicitados)
        {
            if (existentes.Items.Any(x =>
                    EstadosActivosReserva.Contains(x.EstadoReserva) &&
                    x.Detalles.Any(d => !d.EsEliminado && d.IdPasajero == detalle.IdPasajero)))
            {
                throw new BusinessException($"El pasajero {detalle.IdPasajero} ya tiene una reserva activa en este vuelo.");
            }

            if (existentes.Items.Any(x =>
                    EstadosActivosReserva.Contains(x.EstadoReserva) &&
                    x.Detalles.Any(d => !d.EsEliminado && d.IdAsiento == detalle.IdAsiento)))
            {
                throw new BusinessException($"El asiento {detalle.IdAsiento} ya fue reservado en este vuelo.");
            }
        }

        var detallesActivosExistentes = existentes.Items
            .Where(x => EstadosActivosReserva.Contains(x.EstadoReserva))
            .Sum(x => x.Detalles.Count(d => !d.EsEliminado));

        if (detallesActivosExistentes + detallesSolicitados.Count > vuelo.CapacidadTotal)
            throw new BusinessException("El vuelo no tiene capacidad suficiente para la cantidad de pasajeros solicitada.");

        var dataModel = ReservaBusinessMapper.ToDataModel(request, creadoPorUsuario);

        dataModel.FechaInicio = vuelo.FechaHoraSalida;
        dataModel.FechaFin = vuelo.FechaHoraLlegada;

        var creada = await _reservaDataService.CreateAsync(dataModel);

        return ReservaBusinessMapper.ToResponseDto(creada);
    }

    public async Task<ReservaResponseDto?> UpdateEstadoAsync(
        int idReserva,
        ReservaUpdateRequestDto request,
        string modificadoPorUsuario,
        int? idClienteDelToken,
        string rolDelToken)
    {
        if (idReserva <= 0)
            throw new ValidationException("El id de la reserva debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        _validator.ValidateUpdate(request);

        var actual = await _reservaDataService.GetByIdAsync(idReserva);
        if (actual == null)
            throw new NotFoundException("Reserva no encontrada.");

        var estadoNuevo = request.EstadoReserva.Trim().ToUpperInvariant();

        if (rolDelToken == "CLIENTE")
        {
            if (idClienteDelToken == null || actual.IdCliente != idClienteDelToken)
                throw new UnauthorizedBusinessException("No tienes permiso para modificar esta reserva.");

            if (estadoNuevo != "CAN")
                throw new BusinessException("Como cliente solo puedes cancelar tu propia reserva mediante este endpoint.");
        }

        var estadoActual = actual.EstadoReserva.Trim().ToUpperInvariant();

        if (estadoActual == estadoNuevo)
            return ReservaBusinessMapper.ToResponseDto(actual);

        var transicionesPermitidas = new Dictionary<string, string[]>
        {
            { "PEN", ["CON", "CAN"] },
            { "CON", ["CAN", "EMI", "FIN"] },
            { "EMI", ["FIN"] },
            { "CAN", [] },
            { "FIN", [] }
        };

        if (!transicionesPermitidas.TryGetValue(estadoActual, out var permitidos))
            throw new ValidationException($"Estado actual de la reserva desconocido: {estadoActual}.");

        if (!permitidos.Contains(estadoNuevo))
            throw new BusinessException($"No es posible cambiar el estado de '{estadoActual}' a '{estadoNuevo}'.");

        var vuelo = await _vueloIntegrationService.ObtenerVueloAsync(actual.IdVuelo);
        if (vuelo == null)
            throw new NotFoundException("El vuelo asociado a la reserva no existe.");

        if (estadoNuevo == "FIN" && vuelo.EstadoVuelo != "ATERRIZADO")
            throw new BusinessException("No se puede finalizar la reserva porque el vuelo aún no ha aterrizado.");

        actual.EstadoReserva = estadoNuevo;
        actual.ModificadoPorUsuario = modificadoPorUsuario;
        actual.FechaModificacionUtc = DateTime.UtcNow;
        actual.MotivoCancelacion = estadoNuevo == "CAN"
            ? request.MotivoCancelacion?.Trim()
            : null;
        actual.FechaCancelacionUtc = estadoNuevo == "CAN"
            ? DateTime.UtcNow
            : null;
        actual.FechaConfirmacionUtc = estadoNuevo == "CON"
            ? DateTime.UtcNow
            : actual.FechaConfirmacionUtc;

        var actualizado = await _reservaDataService.UpdateAsync(actual);

        return actualizado == null
            ? null
            : ReservaBusinessMapper.ToResponseDto(actualizado);
    }

    public async Task<ReservaPagarResponseDto?> PagarAsync(
        int idReserva,
        ReservaPagarRequestDto request,
        string modificadoPorUsuario,
        int? idClienteDelToken,
        string rolDelToken)
    {
        if (idReserva <= 0)
            throw new ValidationException("El id de la reserva debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario que ejecuta el pago.");

        request ??= new ReservaPagarRequestDto();

        if (request.CargoServicio < 0)
            throw new ValidationException("El cargo de servicio no puede ser negativo.");

        if (request.Equipaje.Any(x => x.IdDetalle <= 0))
            throw new ValidationException("Cada equipaje debe indicar un id_detalle válido.");

        var reserva = await _reservaDataService.GetByIdAsync(idReserva);
        if (reserva == null)
            throw new NotFoundException("Reserva no encontrada.");

        if (rolDelToken == "CLIENTE" &&
            (idClienteDelToken == null || reserva.IdCliente != idClienteDelToken))
        {
            throw new UnauthorizedBusinessException("No tienes permiso para pagar esta reserva.");
        }

        var estadoReserva = reserva.EstadoReserva.Trim().ToUpperInvariant();

        if (estadoReserva == "CAN" || estadoReserva == "FIN")
            throw new BusinessException("La reserva no puede pagarse en su estado actual.");

        if (estadoReserva == "EMI")
            throw new BusinessException("La reserva ya fue pagada y emitida.");

        if (reserva.Detalles.Count == 0)
            throw new BusinessException("La reserva no tiene detalles para procesar el pago.");

        var vuelo = await _vueloIntegrationService.ObtenerVueloAsync(reserva.IdVuelo);
        if (vuelo == null)
            throw new NotFoundException("El vuelo asociado a la reserva no existe.");

        if (vuelo.Estado != "ACTIVO" || vuelo.EstadoVuelo is not ("PROGRAMADO" or "DEMORADO"))
            throw new BusinessException("Solo se puede pagar una reserva en un vuelo disponible.");

        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var factura = await EnsureFacturaParaPagoAsync(
                reserva,
                request.CargoServicio,
                modificadoPorUsuario);

            if (reserva.EstadoReserva != "CON")
            {
                reserva.EstadoReserva = "CON";
                reserva.FechaConfirmacionUtc ??= DateTime.UtcNow;
                reserva.ModificadoPorUsuario = modificadoPorUsuario;
                reserva.FechaModificacionUtc = DateTime.UtcNow;

                var reservaConfirmada = await _reservaDataService.UpdateAsync(reserva);

                if (reservaConfirmada == null)
                    throw new BusinessException("No se pudo confirmar la reserva antes de emitir los boletos.");

                reserva = reservaConfirmada;
            }

            var boletosActuales = await _boletoService.GetPagedAsync(new BoletoFilterDto
            {
                IdReserva = reserva.IdReserva,
                Page = 1,
                PageSize = 200
            });

            foreach (var detalle in reserva.Detalles.Where(d => !d.EsEliminado))
            {
                var asiento = await _vueloIntegrationService.ObtenerAsientoAsync(detalle.IdAsiento);

                if (asiento == null)
                    throw new NotFoundException($"El asiento {detalle.IdAsiento} no existe.");

                if (asiento.IdVuelo != reserva.IdVuelo)
                    throw new BusinessException($"El asiento {detalle.IdAsiento} no pertenece al vuelo de la reserva.");

                var boletoExistente = boletosActuales.Items.FirstOrDefault(x => x.IdDetalle == detalle.IdDetalle);

                if (boletoExistente != null)
                    continue;

                if (!asiento.Disponible)
                    throw new BusinessException($"El asiento {detalle.IdAsiento} ya no está disponible para completar el pago.");

                await _vueloIntegrationService.MarcarAsientoNoDisponibleAsync(
                    detalle.IdAsiento,
                    modificadoPorUsuario);

                await _boletoService.CreateAsync(new BoletoRequestDto
                {
                    IdReserva = reserva.IdReserva,
                    IdDetalle = detalle.IdDetalle,
                    IdVuelo = reserva.IdVuelo,
                    IdAsiento = detalle.IdAsiento,
                    IdFactura = factura.IdFactura,
                    Clase = asiento.Clase,
                    PrecioVueloBase = Math.Round(vuelo.PrecioBase, 2, MidpointRounding.AwayFromZero),
                    PrecioAsientoExtra = Math.Round(asiento.PrecioExtra, 2, MidpointRounding.AwayFromZero),
                    ImpuestosBoleto = Math.Round(detalle.ValorIvaLinea, 2, MidpointRounding.AwayFromZero),
                    CargoEquipaje = 0m,
                    PrecioFinal = Math.Round(
                        vuelo.PrecioBase +
                        asiento.PrecioExtra +
                        detalle.ValorIvaLinea,
                        2,
                        MidpointRounding.AwayFromZero)
                }, modificadoPorUsuario);
            }

            var boletosFinales = await _boletoService.GetPagedAsync(new BoletoFilterDto
            {
                IdReserva = reserva.IdReserva,
                Page = 1,
                PageSize = 200
            });

            var equipajesCreados = new List<EquipajeResponseDto>();

            foreach (var equipaje in request.Equipaje)
            {
                if (!reserva.Detalles.Any(d => !d.EsEliminado && d.IdDetalle == equipaje.IdDetalle))
                    throw new BusinessException($"El detalle {equipaje.IdDetalle} no pertenece a la reserva.");

                var boleto = boletosFinales.Items.FirstOrDefault(x => x.IdDetalle == equipaje.IdDetalle);

                if (boleto == null)
                    throw new BusinessException($"No se encontró boleto emitido para el detalle {equipaje.IdDetalle}.");

                var creado = await _equipajeService.CreateAsync(new EquipajeRequestDto
                {
                    IdBoleto = boleto.IdBoleto,
                    Tipo = equipaje.Tipo,
                    PesoKg = equipaje.PesoKg,
                    DescripcionEquipaje = equipaje.DescripcionEquipaje,
                    PrecioExtra = 0m
                }, modificadoPorUsuario, idClienteDelToken, rolDelToken);

                equipajesCreados.Add(creado);
            }

            factura = await RecalcularFacturaDesdeBoletosAsync(
                factura,
                reserva.IdReserva,
                modificadoPorUsuario);

            reserva.EstadoReserva = "EMI";
            reserva.ModificadoPorUsuario = modificadoPorUsuario;
            reserva.FechaModificacionUtc = DateTime.UtcNow;

            var actualizada = await _reservaDataService.UpdateAsync(reserva);

            if (actualizada == null)
                return null;

            var boletosRespuesta = await _boletoService.GetPagedAsync(new BoletoFilterDto
            {
                IdReserva = reserva.IdReserva,
                Page = 1,
                PageSize = 200
            });

            return new ReservaPagarResponseDto
            {
                Reserva = new ReservaPagoReservaResumenDto
                {
                    IdReserva = actualizada.IdReserva,
                    CodigoReserva = actualizada.CodigoReserva,
                    EstadoReserva = actualizada.EstadoReserva
                },
                Factura = FacturaBusinessMapper.ToResponseDto(factura),
                Boletos = boletosRespuesta.Items.ToList(),
                Equipajes = equipajesCreados
            };
        });
    }

    public async Task<bool> DeleteAsync(int idReserva, string modificadoPorUsuario)
    {
        if (idReserva <= 0)
            throw new ValidationException("El id de la reserva debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        var actual = await _reservaDataService.GetByIdAsync(idReserva);

        if (actual == null)
            throw new NotFoundException("Reserva no encontrada.");

        return await _reservaDataService.DeleteAsync(idReserva, modificadoPorUsuario);
    }

    private static List<ReservaDetalleRequestDto> GetDetallesSolicitados(ReservaRequestDto request)
    {
        if (request.Detalles.Count > 0)
            return request.Detalles;

        var detalleCompatibilidad = new ReservaDetalleRequestDto
        {
            IdPasajero = request.IdPasajero,
            IdAsiento = request.IdAsiento,
            SubtotalLinea = request.SubtotalReserva,
            ValorIvaLinea = request.ValorIva,
            TotalLinea = request.TotalReserva
        };

        return detalleCompatibilidad.IdPasajero > 0 && detalleCompatibilidad.IdAsiento > 0
            ? [detalleCompatibilidad]
            : [];
    }

    private static void EnsureNoDuplicadosEnRequest(List<ReservaDetalleRequestDto> detalles)
    {
        if (detalles.GroupBy(x => x.IdPasajero).Any(g => g.Key > 0 && g.Count() > 1))
            throw new BusinessException("No se puede repetir el mismo pasajero dentro de la reserva.");

        if (detalles.GroupBy(x => x.IdAsiento).Any(g => g.Key > 0 && g.Count() > 1))
            throw new BusinessException("No se puede repetir el mismo asiento dentro de la reserva.");
    }

    private async Task<FacturaDataModel> EnsureFacturaParaPagoAsync(
        ReservaDataModel reserva,
        decimal cargoServicio,
        string usuario)
    {
        var existentes = await _facturaDataService.GetPagedAsync(new FacturaFiltroDataModel
        {
            IdReserva = reserva.IdReserva,
            PageNumber = 1,
            PageSize = 100
        });

        var factura = existentes.Items.FirstOrDefault(x => x.IdReserva == reserva.IdReserva && x.Estado != "INA");

        if (factura == null)
        {
            factura = await _facturaDataService.CreateAsync(new FacturaDataModel
            {
                IdCliente = reserva.IdCliente,
                IdReserva = reserva.IdReserva,
                Subtotal = reserva.SubtotalReserva,
                ValorIva = reserva.ValorIva,
                CargoServicio = Math.Round(cargoServicio, 2, MidpointRounding.AwayFromZero),
                Total = Math.Round(reserva.TotalReserva + cargoServicio, 2, MidpointRounding.AwayFromZero),
                ObservacionesFactura = $"Factura generada al pagar la reserva {reserva.CodigoReserva}.",
                OrigenCanalFactura = reserva.OrigenCanalReserva,
                CreadoPorUsuario = usuario,
                ServicioOrigen = "VUELOS",
                Estado = "APR",
                EsEliminado = false
            });

            return factura;
        }

        factura.CargoServicio = Math.Round(cargoServicio, 2, MidpointRounding.AwayFromZero);
        factura.Total = Math.Round(reserva.TotalReserva + cargoServicio, 2, MidpointRounding.AwayFromZero);
        factura.Estado = "APR";
        factura.ModificadoPorUsuario = usuario;
        factura.FechaModificacionUtc = DateTime.UtcNow;

        var actualizada = await _facturaDataService.UpdateAsync(factura);

        if (actualizada == null)
            throw new BusinessException("No se pudo actualizar la factura de la reserva.");

        return actualizada;
    }

    private async Task<FacturaDataModel> RecalcularFacturaDesdeBoletosAsync(
        FacturaDataModel factura,
        int idReserva,
        string usuario)
    {
        var boletos = await _boletoService.GetPagedAsync(new BoletoFilterDto
        {
            IdReserva = idReserva,
            Page = 1,
            PageSize = 200
        });

        var subtotal = boletos.Items.Sum(x =>
            x.PrecioVueloBase +
            x.PrecioAsientoExtra +
            x.CargoEquipaje);

        var valorIva = boletos.Items.Sum(x => x.ImpuestosBoleto);

        factura.Subtotal = Math.Round(subtotal, 2, MidpointRounding.AwayFromZero);
        factura.ValorIva = Math.Round(valorIva, 2, MidpointRounding.AwayFromZero);
        factura.Total = Math.Round(
            factura.Subtotal + factura.ValorIva + factura.CargoServicio,
            2,
            MidpointRounding.AwayFromZero);
        factura.ModificadoPorUsuario = usuario;
        factura.FechaModificacionUtc = DateTime.UtcNow;

        var actualizada = await _facturaDataService.UpdateAsync(factura);

        if (actualizada == null)
            throw new BusinessException("No se pudo recalcular la factura de la reserva.");

        return actualizada;
    }
}