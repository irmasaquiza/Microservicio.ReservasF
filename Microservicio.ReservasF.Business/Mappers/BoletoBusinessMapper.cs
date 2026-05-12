using Microservicio.ReservasF.Business.DTOs.Boleto;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.Business.Mappers;

public static class BoletoBusinessMapper
{
    public static BoletoFiltroDataModel ToFiltroDataModel(BoletoFilterDto dto)
    {
        return new BoletoFiltroDataModel
        {
            IdReserva = dto.IdReserva,
            IdVuelo = dto.IdVuelo,
            CodigoBoleto = dto.CodigoBoleto,
            EstadoBoleto = dto.EstadoBoleto,
            PageNumber = dto.Page,
            PageSize = dto.PageSize
        };
    }

    public static BoletoDataModel ToDataModel(BoletoRequestDto dto, string creadoPorUsuario)
    {
        return new BoletoDataModel
        {
            IdReserva = dto.IdReserva,
            IdDetalle = dto.IdDetalle,
            IdVuelo = dto.IdVuelo,
            IdAsiento = dto.IdAsiento,
            IdFactura = dto.IdFactura,
            Clase = dto.Clase,
            PrecioVueloBase = dto.PrecioVueloBase,
            PrecioAsientoExtra = dto.PrecioAsientoExtra,
            ImpuestosBoleto = dto.ImpuestosBoleto,
            CargoEquipaje = dto.CargoEquipaje,
            PrecioFinal = dto.PrecioFinal,
            EstadoBoleto = "ACTIVO",
            Estado = "ACTIVO",
            EsEliminado = false,
            CreadoPorUsuario = creadoPorUsuario
        };
    }

    public static BoletoDataModel ToDataModel(int idBoleto, BoletoUpdateRequestDto dto, string modificadoPorUsuario)
    {
        return new BoletoDataModel
        {
            IdBoleto = idBoleto,
            EstadoBoleto = dto.EstadoBoleto,
            ModificadoPorUsuario = modificadoPorUsuario
        };
    }

    public static BoletoResponseDto ToResponseDto(BoletoDataModel model)
    {
        return new BoletoResponseDto
        {
            IdBoleto = model.IdBoleto,
            CodigoBoleto = model.CodigoBoleto,
            IdReserva = model.IdReserva,
            IdDetalle = model.IdDetalle,
            IdVuelo = model.IdVuelo,
            IdAsiento = model.IdAsiento,
            IdFactura = model.IdFactura,
            Clase = model.Clase,
            PrecioVueloBase = model.PrecioVueloBase,
            PrecioAsientoExtra = model.PrecioAsientoExtra,
            ImpuestosBoleto = model.ImpuestosBoleto,
            CargoEquipaje = model.CargoEquipaje,
            PrecioFinal = model.PrecioFinal,
            EstadoBoleto = model.EstadoBoleto,
            FechaEmision = model.FechaEmision
        };
    }

    public static List<BoletoResponseDto> ToResponseDtoList(IEnumerable<BoletoDataModel> items)
    {
        return items.Select(ToResponseDto).ToList();
    }
}