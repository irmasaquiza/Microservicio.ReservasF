using System;
using Microservicio.ReservasF.Business.DTOs.Reserva;
using Microservicio.ReservasF.Business.DTOs.ReservaDetalle;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.Business.Mappers;

public static class ReservaBusinessMapper
{
    private const decimal IvaRate = 0.15m;

    public static ReservaFiltroDataModel ToFiltroDataModel(ReservaFilterDto dto)
    {
        return new ReservaFiltroDataModel
        {
            CodigoReserva = dto.CodigoReserva,
            IdCliente = dto.IdCliente,
            IdPasajero = dto.IdPasajero,
            IdVuelo = dto.IdVuelo,
            EstadoReserva = dto.EstadoReserva,
            PageNumber = dto.Page,
            PageSize = dto.PageSize
        };
    }

    public static ReservaDataModel ToDataModel(ReservaRequestDto dto, string creadoPorUsuario)
    {
        var subtotal = Math.Round(dto.SubtotalReserva, 2, MidpointRounding.AwayFromZero);
        var valorIva = Math.Round(subtotal * IvaRate, 2, MidpointRounding.AwayFromZero);
        var total = Math.Round(subtotal + valorIva, 2, MidpointRounding.AwayFromZero);

        var detalles = dto.Detalles.Any()
            ? dto.Detalles
            : new List<ReservaDetalleRequestDto>
            {
                new()
                {
                    IdPasajero = dto.IdPasajero,
                    IdAsiento = dto.IdAsiento,
                    SubtotalLinea = subtotal,
                    ValorIvaLinea = valorIva,
                    TotalLinea = total
                }
            };

        return new ReservaDataModel
        {
            IdCliente = dto.IdCliente,
            IdVuelo = dto.IdVuelo,
            FechaInicio = dto.FechaInicio ?? default,
            FechaFin = dto.FechaFin ?? default,
            SubtotalReserva = subtotal,
            ValorIva = valorIva,
            TotalReserva = total,
            OrigenCanalReserva = string.IsNullOrWhiteSpace(dto.OrigenCanalReserva)
                ? "BOOKING"
                : dto.OrigenCanalReserva,
            ContactoEmail = dto.ContactoEmail,
            ContactoTelefono = dto.ContactoTelefono,
            Observaciones = dto.Observaciones,
            EstadoReserva = "PEN",
            EsEliminado = false,
            CreadoPorUsuario = creadoPorUsuario,
            ServicioOrigen = "VUELOS",
            Detalles = detalles.Select(d => new ReservaDetalleDataModel
            {
                IdPasajero = d.IdPasajero,
                IdAsiento = d.IdAsiento,
                SubtotalLinea = Math.Round(d.SubtotalLinea, 2, MidpointRounding.AwayFromZero),
                ValorIvaLinea = Math.Round(d.ValorIvaLinea, 2, MidpointRounding.AwayFromZero),
                TotalLinea = Math.Round(d.TotalLinea, 2, MidpointRounding.AwayFromZero),
                Estado = "ACTIVO",
                EsEliminado = false,
                CreadoPorUsuario = creadoPorUsuario
            }).ToList()
        };
    }

    public static ReservaDataModel ToDataModel(int idReserva, ReservaUpdateRequestDto dto, string modificadoPorUsuario)
    {
        return new ReservaDataModel
        {
            IdReserva = idReserva,
            EstadoReserva = dto.EstadoReserva,
            MotivoCancelacion = dto.MotivoCancelacion,
            ModificadoPorUsuario = modificadoPorUsuario
        };
    }

    public static ReservaResponseDto ToResponseDto(ReservaDataModel model)
    {
        var primerDetalle = model.Detalles.FirstOrDefault(d => !d.EsEliminado);

        return new ReservaResponseDto
        {
            IdReserva = model.IdReserva,
            GuidReserva = model.GuidReserva,
            CodigoReserva = model.CodigoReserva,
            IdCliente = model.IdCliente,

            // Campos puente temporales: salen del primer detalle.
            IdPasajero = primerDetalle?.IdPasajero ?? 0,
            IdAsiento = primerDetalle?.IdAsiento ?? 0,

            IdVuelo = model.IdVuelo,
            FechaReservaUtc = model.FechaReservaUtc,
            FechaInicio = model.FechaInicio,
            FechaFin = model.FechaFin,
            SubtotalReserva = model.SubtotalReserva,
            ValorIva = model.ValorIva,
            TotalReserva = model.TotalReserva,
            OrigenCanalReserva = model.OrigenCanalReserva,
            EstadoReserva = model.EstadoReserva,
            FechaConfirmacionUtc = model.FechaConfirmacionUtc,
            FechaCancelacionUtc = model.FechaCancelacionUtc,
            MotivoCancelacion = model.MotivoCancelacion,
            ContactoEmail = model.ContactoEmail,
            ContactoTelefono = model.ContactoTelefono,
            Observaciones = model.Observaciones,
            Detalles = model.Detalles
                .Where(d => !d.EsEliminado)
                .Select(d => new ReservaDetalleResponseDto
                {
                    IdDetalle = d.IdDetalle,
                    IdPasajero = d.IdPasajero,
                    IdAsiento = d.IdAsiento,
                    SubtotalLinea = d.SubtotalLinea,
                    ValorIvaLinea = d.ValorIvaLinea,
                    TotalLinea = d.TotalLinea,
                    EsEliminado = d.EsEliminado
                }).ToList()
        };
    }

    public static List<ReservaResponseDto> ToResponseDtoList(IEnumerable<ReservaDataModel> items)
    {
        return items.Select(ToResponseDto).ToList();
    }
}