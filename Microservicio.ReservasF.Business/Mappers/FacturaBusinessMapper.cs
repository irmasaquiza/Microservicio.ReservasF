using System;
using Microservicio.ReservasF.Business.DTOs.Factura;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.Business.Mappers;

public static class FacturaBusinessMapper
{
    private const decimal IvaRate = 0.15m;

    public static FacturaFiltroDataModel ToFiltroDataModel(FacturaFilterDto dto)
    {
        return new FacturaFiltroDataModel
        {
            NumeroFactura = dto.NumeroFactura,
            IdCliente = dto.IdCliente,
            IdReserva = dto.IdReserva,
            EstadoFactura = dto.Estado,
            PageNumber = dto.Page,
            PageSize = dto.PageSize
        };
    }

    public static FacturaDataModel ToDataModel(FacturaRequestDto dto, string creadoPorUsuario)
    {
        var subtotal = Math.Round(dto.Subtotal, 2, MidpointRounding.AwayFromZero);
        var valorIva = Math.Round(subtotal * IvaRate, 2, MidpointRounding.AwayFromZero);
        var cargoServicio = Math.Round(dto.CargoServicio, 2, MidpointRounding.AwayFromZero);
        var total = Math.Round(subtotal + valorIva + cargoServicio, 2, MidpointRounding.AwayFromZero);

        return new FacturaDataModel
        {
            IdCliente = dto.IdCliente,
            IdReserva = dto.IdReserva,
            Subtotal = subtotal,
            ValorIva = valorIva,
            CargoServicio = cargoServicio,
            Total = total,
            ObservacionesFactura = dto.ObservacionesFactura,
            EstadoFactura = "ABI",
            EsEliminado = false,
            CreadoPorUsuario = creadoPorUsuario,
            ServicioOrigen = "VUELOS"
        };
    }

    public static FacturaDataModel ToDataModel(int idFactura, FacturaUpdateRequestDto dto, string modificadoPorUsuario)
    {
        return new FacturaDataModel
        {
            IdFactura = idFactura,
            EstadoFactura = dto.Estado,
            MotivoInhabilitacion = dto.MotivoInhabilitacion,
            ModificadoPorUsuario = modificadoPorUsuario
        };
    }

    public static FacturaResponseDto ToResponseDto(FacturaDataModel model)
    {
        return new FacturaResponseDto
        {
            IdFactura = model.IdFactura,
            GuidFactura = model.GuidFactura,
            NumeroFactura = model.NumeroFactura,
            IdCliente = model.IdCliente,
            IdReserva = model.IdReserva,
            FechaEmision = model.FechaEmision,
            Subtotal = model.Subtotal,
            ValorIva = model.ValorIva,
            CargoServicio = model.CargoServicio,
            Total = model.Total,
            Estado = model.EstadoFactura,
            ObservacionesFactura = model.ObservacionesFactura
        };
    }

    public static List<FacturaResponseDto> ToResponseDtoList(IEnumerable<FacturaDataModel> items)
    {
        return items.Select(ToResponseDto).ToList();
    }
}