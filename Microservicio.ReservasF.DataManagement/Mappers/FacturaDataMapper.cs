using Microservicio.ReservasF.DataAccess.Entities;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.DataManagement.Mappers;

public static class FacturaDataMapper
{
    // ============================================
    // ENTITY -> DATA MODEL
    // ============================================

    public static FacturaDataModel ToDataModel(
        FacturaEntity entity)
    {
        return new FacturaDataModel
        {
            IdFactura = entity.IdFactura,
            GuidFactura = entity.GuidFactura,

            IdCliente = entity.IdCliente,
            IdReserva = entity.IdReserva,

            NumeroFactura = entity.NumeroFactura,

            FechaEmision = entity.FechaEmision,

            Subtotal = entity.Subtotal,
            ValorIva = entity.ValorIva,
            CargoServicio = entity.CargoServicio,
            Total = entity.Total,

            ObservacionesFactura =
                entity.ObservacionesFactura,

            OrigenCanalFactura =
                entity.OrigenCanalFactura,

            EstadoFactura =
                entity.Estado,

            FechaInhabilitacionUtc =
                entity.FechaInhabilitacionUtc,

            EsEliminado =
                entity.EsEliminado,

            CreadoPorUsuario =
                entity.CreadoPorUsuario,

            FechaRegistroUtc =
                entity.FechaRegistroUtc,

            ModificadoPorUsuario =
                entity.ModificadoPorUsuario,

            FechaModificacionUtc =
                entity.FechaModificacionUtc,

            ModificacionIp =
                entity.ModificacionIp,

            ServicioOrigen =
                entity.ServicioOrigen,

            MotivoInhabilitacion =
                entity.MotivoInhabilitacion,

        };
    }

    // ============================================
    // DATA MODEL -> ENTITY
    // ============================================

    public static FacturaEntity ToEntity(
        FacturaDataModel model)
    {
        return new FacturaEntity
        {
            IdFactura = model.IdFactura,
            GuidFactura = model.GuidFactura,

            IdCliente = model.IdCliente,
            IdReserva = model.IdReserva,

            NumeroFactura = model.NumeroFactura,

            FechaEmision = model.FechaEmision,

            Subtotal = model.Subtotal,
            ValorIva = model.ValorIva,
            CargoServicio = model.CargoServicio,
            Total = model.Total,

            ObservacionesFactura =
                model.ObservacionesFactura,

            OrigenCanalFactura =
                model.OrigenCanalFactura,

            Estado =
                model.EstadoFactura,

            FechaInhabilitacionUtc =
                model.FechaInhabilitacionUtc,

            EsEliminado =
                model.EsEliminado,

            CreadoPorUsuario =
                model.CreadoPorUsuario,

            FechaRegistroUtc =
                model.FechaRegistroUtc,

            ModificadoPorUsuario =
                model.ModificadoPorUsuario,

            FechaModificacionUtc =
                model.FechaModificacionUtc,

            ModificacionIp =
                model.ModificacionIp,

            ServicioOrigen =
                model.ServicioOrigen,

            MotivoInhabilitacion =
                model.MotivoInhabilitacion,

        };
    }

    // ============================================
    // UPDATE ENTITY
    // ============================================

    public static void UpdateEntity(
        FacturaEntity entity,
        FacturaDataModel model)
    {
        entity.IdCliente =
            model.IdCliente;

        entity.IdReserva =
            model.IdReserva;

        entity.NumeroFactura =
            model.NumeroFactura;

        entity.FechaEmision =
            model.FechaEmision;

        entity.Subtotal =
            model.Subtotal;

        entity.ValorIva =
            model.ValorIva;

        entity.CargoServicio =
            model.CargoServicio;

        entity.Total =
            model.Total;

        entity.ObservacionesFactura =
            model.ObservacionesFactura;

        entity.OrigenCanalFactura =
            model.OrigenCanalFactura;

        entity.Estado =
            model.EstadoFactura;

        entity.FechaInhabilitacionUtc =
            model.FechaInhabilitacionUtc;

        entity.ModificadoPorUsuario =
            model.ModificadoPorUsuario;

        entity.FechaModificacionUtc =
            model.FechaModificacionUtc;

        entity.ModificacionIp =
            model.ModificacionIp;

        entity.ServicioOrigen =
            model.ServicioOrigen;

        entity.MotivoInhabilitacion =
            model.MotivoInhabilitacion;
    }
}