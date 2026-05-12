using System.Text.Json.Serialization;

namespace Microservicio.ReservasF.Business.DTOs.Reserva;

public class ReservaPagarEquipajeRequestDto
{
    [JsonPropertyName("id_detalle")]
    public int IdDetalle { get; set; }

    [JsonPropertyName("tipo")]
    public string Tipo { get; set; } = null!;

    [JsonPropertyName("peso_kg")]
    public decimal PesoKg { get; set; }

    [JsonPropertyName("descripcion_equipaje")]
    public string? DescripcionEquipaje { get; set; }
}