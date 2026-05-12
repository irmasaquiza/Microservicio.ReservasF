using Microservicio.ReservasF.Business.DTOs.Boleto;
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

public class BoletoService : IBoletoService
{
    private readonly IBoletoDataService _boletoDataService;
    private readonly IReservaDataService _reservaDataService;
    private readonly IFacturaDataService _facturaDataService;
    private readonly IVueloIntegrationService _vueloIntegrationService;
    private readonly BoletoValidator _validator;

    public BoletoService(
        IBoletoDataService boletoDataService,
        IReservaDataService reservaDataService,
        IFacturaDataService facturaDataService,
        IVueloIntegrationService vueloIntegrationService)
    {
        _boletoDataService = boletoDataService;
        _reservaDataService = reservaDataService;
        _facturaDataService = facturaDataService;
        _vueloIntegrationService = vueloIntegrationService;
        _validator = new BoletoValidator();
    }

    public async Task<DataPagedResult<BoletoResponseDto>> GetPagedAsync(BoletoFilterDto filter)
    {
        _validator.ValidateFilter(filter);

        var filtro = BoletoBusinessMapper.ToFiltroDataModel(filter);
        var result = await _boletoDataService.GetPagedAsync(filtro);

        return new DataPagedResult<BoletoResponseDto>
        {
            Items = BoletoBusinessMapper.ToResponseDtoList(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<BoletoResponseDto?> GetByIdAsync(int idBoleto, int? idClienteDelToken, string rolDelToken)
    {
        if (idBoleto <= 0)
            throw new ValidationException("El id del boleto debe ser mayor que 0.");

        var data = await _boletoDataService.GetByIdAsync(idBoleto);

        if (data == null)
            return null;

        if (rolDelToken == "CLIENTE")
        {
            if (idClienteDelToken is null)
                throw new UnauthorizedBusinessException("No se pudo identificar el cliente del token.");

            var reserva = await _reservaDataService.GetByIdAsync(data.IdReserva);

            if (reserva is null || reserva.IdCliente != idClienteDelToken)
                throw new UnauthorizedBusinessException("No tienes permiso para ver este boleto.");
        }

        return BoletoBusinessMapper.ToResponseDto(data);
    }

    public async Task<BoletoResponseDto> CreateAsync(BoletoRequestDto request, string creadoPorUsuario)
    {
        if (string.IsNullOrWhiteSpace(creadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario creador.");

        _validator.ValidateRequest(request);

        var reserva = await _reservaDataService.GetByIdAsync(request.IdReserva);
        if (reserva == null)
            throw new NotFoundException("La reserva indicada no existe.");

        var detalleReserva = reserva.Detalles
            .FirstOrDefault(d => !d.EsEliminado && d.IdDetalle == request.IdDetalle);

        if (detalleReserva == null)
            throw new BusinessException("El detalle de reserva indicado no existe o no pertenece a la reserva.");

        var vueloExiste = await _vueloIntegrationService.ExisteVueloAsync(request.IdVuelo);
        if (!vueloExiste)
            throw new NotFoundException("El vuelo indicado no existe.");

        var vueloValido = await _vueloIntegrationService.VueloPermiteEmisionAsync(request.IdVuelo);
        if (!vueloValido)
            throw new BusinessException("No se puede emitir boleto para un vuelo cancelado o inactivo.");

        var asientoExiste = await _vueloIntegrationService.ExisteAsientoAsync(request.IdAsiento);
        if (!asientoExiste)
            throw new NotFoundException("El asiento indicado no existe.");

        var asientoValido = await _vueloIntegrationService.AsientoPerteneceAVueloAsync(request.IdAsiento, request.IdVuelo);
        if (!asientoValido)
            throw new BusinessException("El asiento indicado no pertenece al vuelo indicado.");

        var factura = await _facturaDataService.GetByIdAsync(request.IdFactura);
        if (factura == null)
            throw new NotFoundException("La factura indicada no existe.");

        if (reserva.IdVuelo != request.IdVuelo)
            throw new BusinessException("La reserva no pertenece al vuelo indicado.");

        if (detalleReserva.IdAsiento != request.IdAsiento)
            throw new BusinessException("El detalle de reserva no pertenece al asiento indicado.");

        if (factura.IdReserva != request.IdReserva)
            throw new BusinessException("La factura no pertenece a la reserva indicada.");

        if (factura.IdCliente != reserva.IdCliente)
            throw new BusinessException("La factura no corresponde al cliente de la reserva.");

        if (reserva.EstadoReserva is not ("CON" or "EMI"))
            throw new BusinessException("Solo se puede emitir boleto para reservas en estado CON o EMI.");

        var estadoFactura = factura.Estado.Trim().ToUpperInvariant();

        if (estadoFactura != "APR")
            throw new BusinessException("Solo se puede emitir boleto con factura aprobada (APR).");

        var existentes = await _boletoDataService.GetPagedAsync(new BoletoFiltroDataModel
        {
            IdReserva = request.IdReserva,
            PageNumber = 1,
            PageSize = 10000
        });

        if (existentes.Items.Any(x => x.IdDetalle == request.IdDetalle))
            throw new BusinessException("Ya existe un boleto para el detalle de reserva indicado.");

        var dataModel = BoletoBusinessMapper.ToDataModel(request, creadoPorUsuario);
        var creado = await _boletoDataService.CreateAsync(dataModel);

        var boletosActivos = existentes.Items.Count(x => x.EstadoBoleto != "CANCELADO") + 1;
        var detallesActivos = reserva.Detalles.Count(x => !x.EsEliminado);

        if (reserva.EstadoReserva.Trim().ToUpperInvariant() == "CON" &&
            detallesActivos > 0 &&
            boletosActivos >= detallesActivos)
        {
            reserva.EstadoReserva = "EMI";
            reserva.ModificadoPorUsuario = creadoPorUsuario;
            reserva.FechaModificacionUtc = DateTime.UtcNow;

            await _reservaDataService.UpdateAsync(reserva);
        }

        return BoletoBusinessMapper.ToResponseDto(creado);
    }

    public async Task<BoletoResponseDto?> UpdateEstadoAsync(
        int idBoleto,
        BoletoUpdateRequestDto request,
        string modificadoPorUsuario)
    {
        if (idBoleto <= 0)
            throw new ValidationException("El id del boleto debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        _validator.ValidateUpdate(request);

        var actual = await _boletoDataService.GetByIdAsync(idBoleto);
        if (actual == null)
            throw new NotFoundException("Boleto no encontrado.");

        var estadoActual = actual.EstadoBoleto.Trim().ToUpperInvariant();
        var estadoNuevo = request.EstadoBoleto.Trim().ToUpperInvariant();

        var transicionesPermitidas = new Dictionary<string, string[]>
        {
            { "ACTIVO", new[] { "USADO", "CANCELADO" } },
            { "USADO", Array.Empty<string>() },
            { "CANCELADO", Array.Empty<string>() }
        };

        if (!transicionesPermitidas.TryGetValue(estadoActual, out var permitidos))
            throw new ValidationException($"Estado actual de boleto desconocido: {estadoActual}.");

        if (!permitidos.Contains(estadoNuevo))
            throw new BusinessException($"No es posible cambiar el estado de '{estadoActual}' a '{estadoNuevo}'.");

        actual.EstadoBoleto = estadoNuevo;
        actual.ModificadoPorUsuario = modificadoPorUsuario;
        actual.FechaModificacionUtc = DateTime.UtcNow;

        var actualizado = await _boletoDataService.UpdateAsync(actual);

        return actualizado == null
            ? null
            : BoletoBusinessMapper.ToResponseDto(actualizado);
    }

    public async Task<bool> DeleteAsync(int idBoleto, string modificadoPorUsuario)
    {
        if (idBoleto <= 0)
            throw new ValidationException("El id del boleto debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        var actual = await _boletoDataService.GetByIdAsync(idBoleto);
        if (actual == null)
            throw new NotFoundException("Boleto no encontrado.");

        return await _boletoDataService.DeleteAsync(idBoleto, modificadoPorUsuario);
    }
}