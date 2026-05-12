using Microservicio.ReservasF.Business.DTOs.Equipaje;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.Business.Mappers;

public static class EquipajeBusinessMapper
{
    public static EquipajeFiltroDataModel ToFiltroDataModel(EquipajeFilterDto dto)
    {
        return new EquipajeFiltroDataModel
        {
            IdBoleto = dto.IdBoleto,
            NumeroEtiqueta = dto.NumeroEtiqueta,
            EstadoEquipaje = dto.EstadoEquipaje,
            Estado = dto.Estado,
            PageNumber = dto.Page,
            PageSize = dto.PageSize
        };
    }

    public static EquipajeDataModel ToDataModel(EquipajeRequestDto dto, string creadoPorUsuario)
    {
        return new EquipajeDataModel
        {
            IdBoleto = dto.IdBoleto,
            Tipo = dto.Tipo,
            PesoKg = dto.PesoKg,
            DescripcionEquipaje = dto.DescripcionEquipaje,
            PrecioExtra = dto.PrecioExtra,
            DimensionesCm = dto.DimensionesCm,
            EstadoEquipaje = "REGISTRADO",
            Estado = "ACTIVO",
            EsEliminado = false,
            CreadoPorUsuario = creadoPorUsuario
        };
    }

    public static EquipajeDataModel ToDataModel(int idEquipaje, EquipajeUpdateRequestDto dto, string modificadoPorUsuario)
    {
        return new EquipajeDataModel
        {
            IdEquipaje = idEquipaje,
            EstadoEquipaje = dto.EstadoEquipaje,
            ModificadoPorUsuario = modificadoPorUsuario
        };
    }

    public static EquipajeResponseDto ToResponseDto(EquipajeDataModel model)
    {
        return new EquipajeResponseDto
        {
            IdEquipaje = model.IdEquipaje,
            IdBoleto = model.IdBoleto,
            Tipo = model.Tipo,
            PesoKg = model.PesoKg,
            DescripcionEquipaje = model.DescripcionEquipaje,
            PrecioExtra = model.PrecioExtra,
            DimensionesCm = model.DimensionesCm,
            NumeroEtiqueta = model.NumeroEtiqueta,
            EstadoEquipaje = model.EstadoEquipaje
        };
    }

    public static List<EquipajeResponseDto> ToResponseDtoList(IEnumerable<EquipajeDataModel> items)
    {
        return items.Select(ToResponseDto).ToList();
    }
}