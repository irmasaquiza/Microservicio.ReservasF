using Microservicio.ReservasF.DataAccess.Entities;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.DataManagement.Mappers;

public static class ReservaDetalleDataMapper
{
    // ============================================
    // ENTITY -> DATA MODEL
    // ============================================

    public static ReservaDetalleDataModel ToDataModel(
        ReservaDetalleEntity entity)
    {
        return new ReservaDetalleDataModel
        {
            IdDetalle = entity.IdDetalle,

            IdReserva = entity.IdReserva,

            IdPasajero = entity.IdPasajero,

            IdAsiento = entity.IdAsiento,

            SubtotalLinea = entity.SubtotalLinea,

            ValorIvaLinea = entity.ValorIvaLinea,

            TotalLinea = entity.TotalLinea,

            Estado = entity.Estado,

            EsEliminado = entity.EsEliminado,

            CreadoPorUsuario =
                entity.CreadoPorUsuario,

            FechaRegistroUtc =
                entity.FechaRegistroUtc,

            ModificadoPorUsuario =
                entity.ModificadoPorUsuario,

            FechaModificacionUtc =
                entity.FechaModificacionUtc,

            ModificacionIp =
                entity.ModificacionIp
        };
    }

    // ============================================
    // DATA MODEL -> ENTITY
    // ============================================

    public static ReservaDetalleEntity ToEntity(
        ReservaDetalleDataModel model)
    {
        return new ReservaDetalleEntity
        {
            IdDetalle = model.IdDetalle,

            IdReserva = model.IdReserva,

            IdPasajero = model.IdPasajero,

            IdAsiento = model.IdAsiento,

            SubtotalLinea = model.SubtotalLinea,

            ValorIvaLinea = model.ValorIvaLinea,

            TotalLinea = model.TotalLinea,

            Estado = model.Estado,

            EsEliminado = model.EsEliminado,

            CreadoPorUsuario =
                model.CreadoPorUsuario,

            FechaRegistroUtc =
                model.FechaRegistroUtc,

            ModificadoPorUsuario =
                model.ModificadoPorUsuario,

            FechaModificacionUtc =
                model.FechaModificacionUtc,

            ModificacionIp =
                model.ModificacionIp
        };
    }

    // ============================================
    // UPDATE ENTITY
    // ============================================

    public static void UpdateEntity(
        ReservaDetalleEntity entity,
        ReservaDetalleDataModel model)
    {
        entity.IdReserva =
            model.IdReserva;

        entity.IdPasajero =
            model.IdPasajero;

        entity.IdAsiento =
            model.IdAsiento;

        entity.SubtotalLinea =
            model.SubtotalLinea;

        entity.ValorIvaLinea =
            model.ValorIvaLinea;

        entity.TotalLinea =
            model.TotalLinea;

        entity.Estado =
            model.Estado;

        entity.ModificadoPorUsuario =
            model.ModificadoPorUsuario;

        entity.FechaModificacionUtc =
            model.FechaModificacionUtc;

        entity.ModificacionIp =
            model.ModificacionIp;
    }
}