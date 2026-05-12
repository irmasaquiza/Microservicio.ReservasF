namespace Microservicio.ReservasF.Business.DTOs.Equipaje;

public class EquipajeRequestDto
{
    public int IdBoleto { get; set; }

    public string Tipo { get; set; } = null!; // MANO / BODEGA

    public decimal PesoKg { get; set; }

    public string? DescripcionEquipaje { get; set; }

    public decimal PrecioExtra { get; set; }

    public string? DimensionesCm { get; set; }
}