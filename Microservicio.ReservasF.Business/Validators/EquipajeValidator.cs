using Microservicio.ReservasF.Business.DTOs.Equipaje;
using Microservicio.ReservasF.Business.Exceptions;

namespace Microservicio.ReservasF.Business.Validators;

public class EquipajeValidator
{
    private static readonly string[] TiposValidos =
    [
        "MANO",
        "BODEGA"
    ];

    private static readonly string[] EstadosEquipajeValidos =
    [
        "REGISTRADO",
        "EMBARCADO",
        "EN_TRANSITO",
        "ENTREGADO",
        "CANCELADO",
        "PERDIDO",
        "DANADO"
    ];

    public void ValidateRequest(EquipajeRequestDto dto)
    {
        var errors = new List<string>();

        if (dto.IdBoleto <= 0)
        {
            errors.Add("El boleto es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(dto.Tipo))
        {
            errors.Add("El tipo de equipaje es obligatorio.");
        }
        else
        {
            var tipo = dto.Tipo
                .Trim()
                .ToUpperInvariant();

            if (!TiposValidos.Contains(tipo))
            {
                errors.Add("El tipo de equipaje debe ser MANO o BODEGA.");
            }

            if (tipo == "MANO" && dto.PesoKg > 10)
            {
                errors.Add("El equipaje de mano no puede superar 10 kg.");
            }

            if (tipo == "BODEGA" && dto.PesoKg > 23)
            {
                errors.Add("El equipaje de bodega no puede superar 23 kg.");
            }
        }

        if (dto.PesoKg <= 0)
        {
            errors.Add("El peso del equipaje debe ser mayor que 0.");
        }

        if (!string.IsNullOrWhiteSpace(dto.DescripcionEquipaje) &&
            dto.DescripcionEquipaje.Trim().Length > 150)
        {
            errors.Add("La descripción del equipaje no puede exceder 150 caracteres.");
        }

        if (dto.PrecioExtra < 0)
        {
            errors.Add("El precio extra del equipaje no puede ser negativo.");
        }

        if (!string.IsNullOrWhiteSpace(dto.DimensionesCm) &&
            dto.DimensionesCm.Trim().Length > 50)
        {
            errors.Add("Las dimensiones del equipaje no pueden exceder 50 caracteres.");
        }

        ThrowIfAny(
            errors,
            "Error de validación al registrar el equipaje.");
    }

    public void ValidateUpdate(EquipajeUpdateRequestDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.EstadoEquipaje))
        {
            errors.Add("El estado del equipaje es obligatorio.");
        }
        else
        {
            var estado = dto.EstadoEquipaje
                .Trim()
                .ToUpperInvariant();

            if (!EstadosEquipajeValidos.Contains(estado))
            {
                errors.Add("El estado del equipaje debe ser REGISTRADO, EMBARCADO, EN_TRANSITO, ENTREGADO, CANCELADO, PERDIDO o DANADO.");
            }
        }

        ThrowIfAny(
            errors,
            "Error de validación al actualizar el equipaje.");
    }

    public void ValidateFilter(EquipajeFilterDto dto)
    {
        var errors = new List<string>();

        if (dto.IdBoleto.HasValue &&
            dto.IdBoleto.Value <= 0)
        {
            errors.Add("El id del boleto debe ser mayor que 0.");
        }

        if (!string.IsNullOrWhiteSpace(dto.NumeroEtiqueta) &&
            dto.NumeroEtiqueta.Trim().Length > 50)
        {
            errors.Add("El número de etiqueta no puede exceder 50 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(dto.EstadoEquipaje))
        {
            var estadoEquipaje = dto.EstadoEquipaje
                .Trim()
                .ToUpperInvariant();

            if (!EstadosEquipajeValidos.Contains(estadoEquipaje))
            {
                errors.Add("El estado del equipaje debe ser REGISTRADO, EMBARCADO, EN_TRANSITO, ENTREGADO, CANCELADO, PERDIDO o DANADO.");
            }
        }

        if (!string.IsNullOrWhiteSpace(dto.Estado))
        {
            var estado = dto.Estado
                .Trim()
                .ToUpperInvariant();

            if (estado != "ACTIVO" &&
                estado != "INACTIVO")
            {
                errors.Add("El estado debe ser ACTIVO o INACTIVO.");
            }
        }

        if (dto.Page <= 0)
        {
            errors.Add("La página debe ser mayor que 0.");
        }

        if (dto.PageSize <= 0 ||
            dto.PageSize > 200)
        {
            errors.Add("El tamaño de página debe estar entre 1 y 200.");
        }

        ThrowIfAny(
            errors,
            "Error de validación en el filtro de equipajes.");
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