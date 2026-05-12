using Microservicio.ReservasF.Business.DTOs.Factura;
using Microservicio.ReservasF.Business.Exceptions;

namespace Microservicio.ReservasF.Business.Validators;

public class FacturaValidator
{
    private static readonly string[] EstadosValidos =
    [
        "ABI",
        "APR",
        "INA"
    ];

    public void ValidateRequest(FacturaRequestDto dto)
    {
        var errors = new List<string>();

        if (dto.IdCliente <= 0)
            errors.Add("El cliente es obligatorio.");

        if (dto.IdReserva <= 0)
            errors.Add("La reserva es obligatoria.");

        if (dto.Subtotal < 0)
            errors.Add("El subtotal no puede ser negativo.");

        if (dto.ValorIva < 0)
            errors.Add("El IVA no puede ser negativo.");

        if (dto.CargoServicio < 0)
            errors.Add("El cargo de servicio no puede ser negativo.");

        if (dto.Total < 0)
            errors.Add("El total no puede ser negativo.");

        if (!string.IsNullOrWhiteSpace(dto.ObservacionesFactura) &&
            dto.ObservacionesFactura.Trim().Length > 300)
        {
            errors.Add("Las observaciones de la factura no pueden exceder 300 caracteres.");
        }

        ThrowIfAny(
            errors,
            "Error de validación al crear la factura.");
    }

    public void ValidateUpdate(FacturaUpdateRequestDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Estado))
        {
            errors.Add("El estado es obligatorio.");
        }
        else
        {
            var estado = dto.Estado.Trim().ToUpperInvariant();

            if (!EstadosValidos.Contains(estado))
                errors.Add("El estado debe ser ABI, APR o INA.");
        }

        if (!string.IsNullOrWhiteSpace(dto.MotivoInhabilitacion) &&
            dto.MotivoInhabilitacion.Trim().Length > 250)
        {
            errors.Add("El motivo de inhabilitación no puede exceder 250 caracteres.");
        }

        ThrowIfAny(
            errors,
            "Error de validación al actualizar la factura.");
    }

    public void ValidateFilter(FacturaFilterDto dto)
    {
        var errors = new List<string>();

        if (!string.IsNullOrWhiteSpace(dto.NumeroFactura) &&
            dto.NumeroFactura.Trim().Length > 40)
        {
            errors.Add("El número de factura no puede exceder 40 caracteres.");
        }

        if (dto.IdCliente.HasValue &&
            dto.IdCliente.Value <= 0)
        {
            errors.Add("El id del cliente debe ser mayor que 0.");
        }

        if (dto.IdReserva.HasValue &&
            dto.IdReserva.Value <= 0)
        {
            errors.Add("El id de la reserva debe ser mayor que 0.");
        }

        if (!string.IsNullOrWhiteSpace(dto.Estado))
        {
            var estado = dto.Estado.Trim().ToUpperInvariant();

            if (!EstadosValidos.Contains(estado))
                errors.Add("El estado debe ser ABI, APR o INA.");
        }

        if (dto.Page <= 0)
            errors.Add("La página debe ser mayor que 0.");

        if (dto.PageSize <= 0 ||
            dto.PageSize > 200)
        {
            errors.Add("El tamaño de página debe estar entre 1 y 200.");
        }

        ThrowIfAny(
            errors,
            "Error de validación en el filtro de facturas.");
    }

    private static void ThrowIfAny(
        List<string> errors,
        string message)
    {
        if (errors.Count > 0)
        {
            throw new ValidationException(
                message,
                errors);
        }
    }
}