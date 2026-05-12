using Microservicio.ReservasF.Business.DTOs.Boleto;
using Microservicio.ReservasF.Business.DTOs.Equipaje;
using Microservicio.ReservasF.Business.DTOs.Factura;

namespace Microservicio.ReservasF.Business.DTOs.Reserva;

public class ReservaPagarResponseDto
{
    public ReservaPagoReservaResumenDto Reserva { get; set; } = null!;

    public FacturaResponseDto Factura { get; set; } = null!;

    public List<BoletoResponseDto> Boletos { get; set; } = new();

    public List<EquipajeResponseDto> Equipajes { get; set; } = new();
}