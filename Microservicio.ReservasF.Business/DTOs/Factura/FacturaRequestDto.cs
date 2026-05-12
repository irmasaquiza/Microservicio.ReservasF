namespace Microservicio.ReservasF.Business.DTOs.Factura;

public class FacturaRequestDto
{
    public int IdCliente { get; set; }

    public int IdReserva { get; set; }

    public decimal Subtotal { get; set; }

    public decimal ValorIva { get; set; }

    public decimal CargoServicio { get; set; }

    public decimal Total { get; set; }

    public string? ObservacionesFactura { get; set; }
}