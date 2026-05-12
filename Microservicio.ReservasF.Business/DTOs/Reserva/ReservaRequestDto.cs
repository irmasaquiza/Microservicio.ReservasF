using System.Text.Json.Serialization;
using Microservicio.ReservasF.Business.DTOs.ReservaDetalle;

namespace Microservicio.ReservasF.Business.DTOs.Reserva;

public class ReservaRequestDto
{
    [JsonPropertyName("id_cliente")]
    public int IdCliente { get; set; }

    [JsonPropertyName("id_pasajero")]
    public int IdPasajero { get; set; }

    [JsonPropertyName("id_vuelo")]
    public int IdVuelo { get; set; }

    [JsonPropertyName("id_asiento")]
    public int IdAsiento { get; set; }

    [JsonPropertyName("fecha_inicio")]
    public DateTime? FechaInicio { get; set; }

    [JsonPropertyName("fecha_fin")]
    public DateTime? FechaFin { get; set; }

    [JsonPropertyName("subtotal_reserva")]
    public decimal SubtotalReserva { get; set; }

    [JsonPropertyName("valor_iva")]
    public decimal ValorIva { get; set; }

    [JsonPropertyName("total_reserva")]
    public decimal TotalReserva { get; set; }

    [JsonPropertyName("origen_canal_reserva")]
    public string? OrigenCanalReserva { get; set; }

    [JsonPropertyName("contacto_email")]
    public string? ContactoEmail { get; set; }

    [JsonPropertyName("contacto_telefono")]
    public string? ContactoTelefono { get; set; }

    [JsonPropertyName("observaciones")]
    public string? Observaciones { get; set; }

    [JsonPropertyName("detalles")]
    public List<ReservaDetalleRequestDto> Detalles { get; set; } = new();

    // Compatibilidad con el payload del front admin que envía la colección como "pasajeros".
    [JsonPropertyName("pasajeros")]
    public List<ReservaDetalleRequestDto> Pasajeros
    {
        get => Detalles;
        set => Detalles = value ?? new();
    }
}