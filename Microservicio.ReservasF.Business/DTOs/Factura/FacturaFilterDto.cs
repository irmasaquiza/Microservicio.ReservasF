using Microsoft.AspNetCore.Mvc;

namespace Microservicio.ReservasF.Business.DTOs.Factura;

public class FacturaFilterDto
{
    [FromQuery(Name = "numero_factura")]
    public string? NumeroFactura { get; set; }

    [FromQuery(Name = "id_cliente")]
    public int? IdCliente { get; set; }

    [FromQuery(Name = "id_reserva")]
    public int? IdReserva { get; set; }

    [FromQuery(Name = "estado")]
    public string? Estado { get; set; }

    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 20;
}