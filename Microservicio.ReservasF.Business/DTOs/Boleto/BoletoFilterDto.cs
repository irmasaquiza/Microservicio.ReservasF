using Microsoft.AspNetCore.Mvc;

namespace Microservicio.ReservasF.Business.DTOs.Boleto;

public class BoletoFilterDto
{
    [FromQuery(Name = "id_reserva")]
    public int? IdReserva { get; set; }

    [FromQuery(Name = "id_vuelo")]
    public int? IdVuelo { get; set; }

    [FromQuery(Name = "codigo_boleto")]
    public string? CodigoBoleto { get; set; }

    [FromQuery(Name = "estado_boleto")]
    public string? EstadoBoleto { get; set; }

    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 20;
}