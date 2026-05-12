using System.Text.Json.Serialization;

namespace Microservicio.ReservasF.Business.DTOs.ReservaDetalle;

public class ReservaDetalleRequestDto
{
    [JsonPropertyName("id_pasajero")]
    public int IdPasajero { get; set; }

    [JsonPropertyName("id_asiento")]
    public int IdAsiento { get; set; }

    [JsonPropertyName("subtotal_linea")]
    public decimal SubtotalLinea { get; set; }

    [JsonPropertyName("valor_iva_linea")]
    public decimal ValorIvaLinea { get; set; }

    [JsonPropertyName("total_linea")]
    public decimal TotalLinea { get; set; }
}