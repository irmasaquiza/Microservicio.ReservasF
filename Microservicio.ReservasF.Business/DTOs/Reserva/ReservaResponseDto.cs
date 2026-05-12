using Microservicio.ReservasF.Business.DTOs.ReservaDetalle;

namespace Microservicio.ReservasF.Business.DTOs.Reserva;

public class ReservaResponseDto
{
    public int IdReserva { get; set; }

    public Guid GuidReserva { get; set; }

    public string CodigoReserva { get; set; } = null!;

    public int IdCliente { get; set; }

    // Campo puente temporal.
    public int IdPasajero { get; set; }

    public int IdVuelo { get; set; }

    // Campo puente temporal.
    public int IdAsiento { get; set; }

    public DateTime FechaReservaUtc { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public decimal SubtotalReserva { get; set; }

    public decimal ValorIva { get; set; }

    public decimal TotalReserva { get; set; }

    public string OrigenCanalReserva { get; set; } = null!;

    public string EstadoReserva { get; set; } = null!;

    public DateTime? FechaConfirmacionUtc { get; set; }

    public DateTime? FechaCancelacionUtc { get; set; }

    public string? MotivoCancelacion { get; set; }

    public string? ContactoEmail { get; set; }

    public string? ContactoTelefono { get; set; }

    public string? Observaciones { get; set; }

    public List<ReservaDetalleResponseDto> Detalles { get; set; } = new();
}