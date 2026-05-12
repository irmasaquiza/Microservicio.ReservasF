using Microsoft.AspNetCore.Mvc;

namespace Microservicio.ReservasF.Business.DTOs.Equipaje;

public class EquipajeFilterDto
{
    [FromQuery(Name = "id_boleto")]
    public int? IdBoleto { get; set; }

    [FromQuery(Name = "numero_etiqueta")]
    public string? NumeroEtiqueta { get; set; }

    [FromQuery(Name = "estado_equipaje")]
    public string? EstadoEquipaje { get; set; }

    [FromQuery(Name = "estado")]
    public string? Estado { get; set; }

    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 20;
}