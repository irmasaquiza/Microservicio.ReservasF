namespace Microservicio.ReservasF.Business.DTOs.ReservaDetalle;

public class ReservaDetalleResponseDto
{
    public int IdDetalle { get; set; }

    public int IdPasajero { get; set; }

    public int IdAsiento { get; set; }

    public decimal SubtotalLinea { get; set; }

    public decimal ValorIvaLinea { get; set; }

    public decimal TotalLinea { get; set; }

    public bool EsEliminado { get; set; }
}