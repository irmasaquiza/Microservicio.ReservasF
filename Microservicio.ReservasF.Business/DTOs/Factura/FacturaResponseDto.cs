namespace Microservicio.ReservasF.Business.DTOs.Factura;

public class FacturaResponseDto
{
    public int IdFactura { get; set; }

    public Guid GuidFactura { get; set; }

    public string NumeroFactura { get; set; } = null!;

    public int IdCliente { get; set; }

    public int IdReserva { get; set; }

    public DateTime FechaEmision { get; set; }

    public decimal Subtotal { get; set; }

    public decimal ValorIva { get; set; }

    public decimal CargoServicio { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } = null!;

    public string? ObservacionesFactura { get; set; }
}