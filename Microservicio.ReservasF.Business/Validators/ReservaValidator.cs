using System.Text.RegularExpressions;
using Microservicio.ReservasF.Business.DTOs.Reserva;
using Microservicio.ReservasF.Business.Exceptions;

namespace Microservicio.ReservasF.Business.Validators;

public class ReservaValidator
{
    private static readonly string[] EstadosValidos =
    [
        "PEN",
        "CON",
        "CAN",
        "FIN",
        "EMI"
    ];

    private static readonly string[] OrigenesCanalValidos =
    [
        "WEB",
        "APP",
        "BOOKING",
        "TELEFONO",
        "PRESENCIAL"
    ];

    public void ValidateRequest(ReservaRequestDto dto)
    {
        var errors = new List<string>();

        if (dto.IdCliente <= 0)
            errors.Add("El cliente es obligatorio.");

        if (dto.IdVuelo <= 0)
            errors.Add("El vuelo es obligatorio.");

        if (dto.FechaInicio.HasValue &&
            dto.FechaFin.HasValue &&
            dto.FechaFin <= dto.FechaInicio)
        {
            errors.Add("La fecha de fin debe ser mayor que la fecha de inicio.");
        }

        if (dto.SubtotalReserva < 0)
            errors.Add("El subtotal de la reserva no puede ser negativo.");

        if (dto.Detalles is null ||
            dto.Detalles.Count == 0)
        {
            if (dto.IdPasajero <= 0)
                errors.Add("El pasajero es obligatorio.");

            if (dto.IdAsiento <= 0)
                errors.Add("El asiento es obligatorio.");
        }
        else
        {
            var pasajeros = new HashSet<int>();
            var asientos = new HashSet<int>();

            for (var i = 0; i < dto.Detalles.Count; i++)
            {
                var detalle = dto.Detalles[i];

                if (detalle.IdPasajero <= 0)
                {
                    errors.Add($"El pasajero del detalle {i + 1} es obligatorio.");
                }
                else if (!pasajeros.Add(detalle.IdPasajero))
                {
                    errors.Add($"El pasajero del detalle {i + 1} está repetido en la reserva.");
                }

                if (detalle.IdAsiento <= 0)
                {
                    errors.Add($"El asiento del detalle {i + 1} es obligatorio.");
                }
                else if (!asientos.Add(detalle.IdAsiento))
                {
                    errors.Add($"El asiento del detalle {i + 1} está repetido en la reserva.");
                }

                if (detalle.SubtotalLinea < 0)
                    errors.Add($"El subtotal del detalle {i + 1} no puede ser negativo.");

                if (detalle.ValorIvaLinea < 0)
                    errors.Add($"El IVA del detalle {i + 1} no puede ser negativo.");

                if (detalle.TotalLinea < 0)
                    errors.Add($"El total del detalle {i + 1} no puede ser negativo.");
            }
        }

        if (!string.IsNullOrWhiteSpace(dto.OrigenCanalReserva))
        {
            var origen = dto.OrigenCanalReserva.Trim().ToUpperInvariant();

            if (!OrigenesCanalValidos.Contains(origen))
                errors.Add("El origen del canal de reserva debe ser WEB, APP, BOOKING, TELEFONO o PRESENCIAL.");
        }

        if (!string.IsNullOrWhiteSpace(dto.ContactoEmail))
        {
            var correo = dto.ContactoEmail.Trim();

            if (correo.Length > 150)
                errors.Add("El correo de contacto no puede exceder 150 caracteres.");

            if (!IsValidEmail(correo))
                errors.Add("El correo de contacto no tiene un formato válido.");
        }

        if (!string.IsNullOrWhiteSpace(dto.ContactoTelefono) &&
            dto.ContactoTelefono.Trim().Length > 20)
        {
            errors.Add("El teléfono de contacto no puede exceder 20 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(dto.Observaciones) &&
            dto.Observaciones.Trim().Length > 255)
        {
            errors.Add("Las observaciones no pueden exceder 255 caracteres.");
        }

        ThrowIfAny(
            errors,
            "Error de validación al crear la reserva.");
    }

    public void ValidateUpdate(ReservaUpdateRequestDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.EstadoReserva))
        {
            errors.Add("El estado de la reserva es obligatorio.");
        }
        else
        {
            var estado = dto.EstadoReserva.Trim().ToUpperInvariant();

            if (!EstadosValidos.Contains(estado))
                errors.Add("El estado de la reserva debe ser PEN, CON, CAN, FIN o EMI.");

            if (estado == "CAN" &&
                string.IsNullOrWhiteSpace(dto.MotivoCancelacion))
            {
                errors.Add("El motivo de cancelación es obligatorio cuando el estado es CAN.");
            }
        }

        if (!string.IsNullOrWhiteSpace(dto.MotivoCancelacion) &&
            dto.MotivoCancelacion.Trim().Length > 250)
        {
            errors.Add("El motivo de cancelación no puede exceder 250 caracteres.");
        }

        ThrowIfAny(
            errors,
            "Error de validación al actualizar el estado de la reserva.");
    }

    public void ValidateFilter(ReservaFilterDto dto)
    {
        var errors = new List<string>();

        if (!string.IsNullOrWhiteSpace(dto.CodigoReserva))
        {
            var codigoReserva = dto.CodigoReserva.Trim();

            if (codigoReserva.Length > 40)
                errors.Add("El código de reserva no puede exceder 40 caracteres.");

            if (!Regex.IsMatch(codigoReserva, "^[A-Za-z0-9\\-]+$"))
                errors.Add("El código de reserva tiene un formato inválido.");
        }

        if (dto.IdCliente.HasValue &&
            dto.IdCliente.Value <= 0)
        {
            errors.Add("El id del cliente debe ser mayor que 0.");
        }

        if (dto.IdPasajero.HasValue &&
            dto.IdPasajero.Value <= 0)
        {
            errors.Add("El id del pasajero debe ser mayor que 0.");
        }

        if (dto.IdVuelo.HasValue &&
            dto.IdVuelo.Value <= 0)
        {
            errors.Add("El id del vuelo debe ser mayor que 0.");
        }

        if (!string.IsNullOrWhiteSpace(dto.EstadoReserva))
        {
            var estado = dto.EstadoReserva.Trim().ToUpperInvariant();

            if (!EstadosValidos.Contains(estado))
                errors.Add("El estado de la reserva debe ser PEN, CON, CAN, FIN o EMI.");
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
            "Error de validación en el filtro de reservas.");
    }

    private static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(
            email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase,
            TimeSpan.FromMilliseconds(250));
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