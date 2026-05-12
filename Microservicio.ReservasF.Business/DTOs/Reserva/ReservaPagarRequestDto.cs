using System.Text.Json.Serialization;

namespace Microservicio.ReservasF.Business.DTOs.Reserva;

public class ReservaPagarRequestDto
{
    [JsonPropertyName("cargo_servicio")]
    public decimal CargoServicio { get; set; }

    [JsonPropertyName("equipaje")]
    public List<ReservaPagarEquipajeRequestDto> Equipaje { get; set; } = new();
}