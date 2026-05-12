using Microservicio.ReservasF.Business.DTOs.Factura;
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

public class FacturaService : IFacturaService
{
    private readonly IFacturaDataService _facturaDataService;
    private readonly IReservaDataService _reservaDataService;
    private readonly IClienteIntegrationService _clienteIntegrationService;
    private readonly FacturaValidator _validator;

    public FacturaService(
        IFacturaDataService facturaDataService,
        IReservaDataService reservaDataService,
        IClienteIntegrationService clienteIntegrationService)
    {
        _facturaDataService = facturaDataService;
        _reservaDataService = reservaDataService;
        _clienteIntegrationService = clienteIntegrationService;
        _validator = new FacturaValidator();
    }

    public async Task<DataPagedResult<FacturaResponseDto>> GetPagedAsync(FacturaFilterDto filter)
    {
        _validator.ValidateFilter(filter);

        var filtro = FacturaBusinessMapper.ToFiltroDataModel(filter);
        var result = await _facturaDataService.GetPagedAsync(filtro);

        return new DataPagedResult<FacturaResponseDto>
        {
            Items = FacturaBusinessMapper.ToResponseDtoList(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords
        };
    }

    public async Task<FacturaResponseDto?> GetByIdAsync(
        int idFactura,
        int? idClienteDelToken,
        string rolDelToken)
    {
        if (idFactura <= 0)
            throw new ValidationException("El id de la factura debe ser mayor que 0.");

        var data = await _facturaDataService.GetByIdAsync(idFactura);

        if (data == null)
            return null;

        if (rolDelToken == "CLIENTE")
        {
            if (idClienteDelToken == null || data.IdCliente != idClienteDelToken)
                throw new UnauthorizedBusinessException("No tienes permiso para ver esta factura.");
        }

        return FacturaBusinessMapper.ToResponseDto(data);
    }

    public async Task<FacturaResponseDto> CreateAsync(
        FacturaRequestDto request,
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

        var reserva = await _reservaDataService.GetByIdAsync(request.IdReserva);
        if (reserva == null)
            throw new NotFoundException("La reserva indicada no existe.");

        if (reserva.EstadoReserva is "CAN" or "FIN")
            throw new BusinessException("No se puede facturar una reserva cancelada o finalizada.");

        if (reserva.IdCliente != request.IdCliente)
            throw new BusinessException("La reserva no pertenece al cliente indicado.");

        var existentes = await _facturaDataService.GetPagedAsync(new FacturaFiltroDataModel
        {
            IdReserva = request.IdReserva,
            PageNumber = 1,
            PageSize = 10000
        });

        if (existentes.Items.Any(x => x.IdReserva == request.IdReserva && x.Estado != "INA"))
            throw new BusinessException("Ya existe una factura activa para la reserva indicada.");

        var dataModel = FacturaBusinessMapper.ToDataModel(request, creadoPorUsuario);
        var creada = await _facturaDataService.CreateAsync(dataModel);

        return FacturaBusinessMapper.ToResponseDto(creada);
    }

    public async Task<FacturaResponseDto?> UpdateEstadoAsync(
        int idFactura,
        FacturaUpdateRequestDto request,
        string modificadoPorUsuario)
    {
        if (idFactura <= 0)
            throw new ValidationException("El id de la factura debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        _validator.ValidateUpdate(request);

        var actual = await _facturaDataService.GetByIdAsync(idFactura);
        if (actual == null)
            throw new NotFoundException("Factura no encontrada.");

        var estadoActual = actual.Estado.Trim().ToUpperInvariant();
        var estadoNuevo = request.Estado.Trim().ToUpperInvariant();

        var transicionesPermitidas = new Dictionary<string, string[]>
        {
            { "ABI", new[] { "APR", "INA" } },
            { "APR", Array.Empty<string>() },
            { "INA", Array.Empty<string>() }
        };

        if (!transicionesPermitidas.TryGetValue(estadoActual, out var permitidos))
            throw new ValidationException($"Estado actual de factura desconocido: {estadoActual}.");

        if (!permitidos.Contains(estadoNuevo))
            throw new BusinessException($"No es posible cambiar el estado de '{estadoActual}' a '{estadoNuevo}'.");

        var reserva = await _reservaDataService.GetByIdAsync(actual.IdReserva);
        if (reserva == null)
            throw new NotFoundException("La reserva asociada a la factura no existe.");

        if (estadoNuevo == "APR" && reserva.EstadoReserva is ("CAN" or "FIN"))
            throw new BusinessException("No se puede aprobar una factura de una reserva cancelada o finalizada.");

        actual.Estado = estadoNuevo;
        actual.ModificadoPorUsuario = modificadoPorUsuario;
        actual.FechaModificacionUtc = DateTime.UtcNow;
        actual.MotivoInhabilitacion = estadoNuevo == "INA"
            ? request.MotivoInhabilitacion?.Trim()
            : null;
        actual.FechaInhabilitacionUtc = estadoNuevo == "INA"
            ? DateTime.UtcNow
            : null;

        var actualizada = await _facturaDataService.UpdateAsync(actual);

        return actualizada == null
            ? null
            : FacturaBusinessMapper.ToResponseDto(actualizada);
    }

    public Task<FacturaResponseDto?> AprobarAsync(
        int idFactura,
        string modificadoPorUsuario)
    {
        return UpdateEstadoAsync(
            idFactura,
            new FacturaUpdateRequestDto { Estado = "APR" },
            modificadoPorUsuario);
    }

    public async Task<FacturaResponseDto?> PagarAsync(
        int idFactura,
        int? idClienteDelToken,
        string rolDelToken,
        string modificadoPorUsuario)
    {
        var factura = await _facturaDataService.GetByIdAsync(idFactura);
        if (factura == null)
            throw new NotFoundException("Factura no encontrada.");

        if (rolDelToken == "CLIENTE" &&
            (idClienteDelToken == null || factura.IdCliente != idClienteDelToken.Value))
        {
            throw new UnauthorizedBusinessException("No tienes permiso para pagar esta factura.");
        }

        return await UpdateEstadoAsync(
            idFactura,
            new FacturaUpdateRequestDto { Estado = "APR" },
            modificadoPorUsuario);
    }

    public async Task<bool> DeleteAsync(
        int idFactura,
        string modificadoPorUsuario)
    {
        if (idFactura <= 0)
            throw new ValidationException("El id de la factura debe ser mayor que 0.");

        if (string.IsNullOrWhiteSpace(modificadoPorUsuario))
            throw new UnauthorizedBusinessException("No se pudo identificar el usuario modificador.");

        var actual = await _facturaDataService.GetByIdAsync(idFactura);
        if (actual == null)
            throw new NotFoundException("Factura no encontrada.");

        return await _facturaDataService.DeleteAsync(idFactura, modificadoPorUsuario);
    }
}