using Microservicio.ReservasF.Business.DTOs.Boleto;
using Microservicio.ReservasF.Business.Exceptions;

namespace Microservicio.ReservasF.Business.Validators;

public class BoletoValidator
{
    private static readonly string[] ClasesValidas =
    [
        "ECONOMICA",
        "EJECUTIVA",
        "PRIMERA"
    ];

    private static readonly string[] EstadosBoletoValidos =
    [
        "ACTIVO",
        "USADO",
        "CANCELADO"
    ];

    public void ValidateRequest(BoletoRequestDto dto)
    {
        var errors = new List<string>();

        if (dto.IdReserva <= 0)
            errors.Add("La reserva es obligatoria.");

        if (dto.IdDetalle <= 0)
            errors.Add("El detalle de reserva es obligatorio.");

        if (dto.IdVuelo <= 0)
            errors.Add("El vuelo es obligatorio.");

        if (dto.IdAsiento <= 0)
            errors.Add("El asiento es obligatorio.");

        if (dto.IdFactura <= 0)
            errors.Add("La factura es obligatoria.");

        if (string.IsNullOrWhiteSpace(dto.Clase))
        {
            errors.Add("La clase es obligatoria.");
        }
        else
        {
            var clase = dto.Clase.Trim().ToUpperInvariant();

            if (!ClasesValidas.Contains(clase))
                errors.Add("La clase debe ser ECONOMICA, EJECUTIVA o PRIMERA.");
        }

        if (dto.PrecioVueloBase < 0)
            errors.Add("El precio base del vuelo no puede ser negativo.");

        if (dto.PrecioAsientoExtra < 0)
            errors.Add("El precio extra del asiento no puede ser negativo.");

        if (dto.ImpuestosBoleto < 0)
            errors.Add("Los impuestos del boleto no pueden ser negativos.");

        if (dto.CargoEquipaje < 0)
            errors.Add("El cargo de equipaje no puede ser negativo.");

        if (dto.PrecioFinal < 0)
            errors.Add("El precio final no puede ser negativo.");

        ThrowIfAny(
            errors,
            "Error de validación al crear el boleto.");
    }

    public void ValidateUpdate(BoletoUpdateRequestDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.EstadoBoleto))
        {
            errors.Add("El estado del boleto es obligatorio.");
        }
        else
        {
            var estado = dto.EstadoBoleto.Trim().ToUpperInvariant();

            if (!EstadosBoletoValidos.Contains(estado))
                errors.Add("El estado del boleto debe ser ACTIVO, USADO o CANCELADO.");
        }

        ThrowIfAny(
            errors,
            "Error de validación al actualizar el boleto.");
    }

    public void ValidateFilter(BoletoFilterDto dto)
    {
        var errors = new List<string>();

        if (dto.IdReserva.HasValue &&
            dto.IdReserva.Value <= 0)
        {
            errors.Add("El id de la reserva debe ser mayor que 0.");
        }

        if (dto.IdVuelo.HasValue &&
            dto.IdVuelo.Value <= 0)
        {
            errors.Add("El id del vuelo debe ser mayor que 0.");
        }

        if (!string.IsNullOrWhiteSpace(dto.CodigoBoleto) &&
            dto.CodigoBoleto.Trim().Length > 20)
        {
            errors.Add("El código del boleto no puede exceder 20 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(dto.EstadoBoleto))
        {
            var estado = dto.EstadoBoleto.Trim().ToUpperInvariant();

            if (!EstadosBoletoValidos.Contains(estado))
                errors.Add("El estado del boleto debe ser ACTIVO, USADO o CANCELADO.");
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
            "Error de validación en el filtro de boletos.");
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