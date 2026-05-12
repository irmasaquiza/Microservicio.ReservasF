namespace Microservicio.ReservasF.Business.DTOs.Reserva;

public class ReservaPagoReservaResumenDto
{
    public int IdReserva { get; set; }

    public string CodigoReserva { get; set; } = null!;

    public string EstadoReserva { get; set; } = null!;
}