namespace Microservicio.ReservasF.Business.DTOs.Factura;

public class FacturaUpdateRequestDto
{
    public string Estado { get; set; } = null!; // ABI / APR / INA

    public string? MotivoInhabilitacion { get; set; }
}