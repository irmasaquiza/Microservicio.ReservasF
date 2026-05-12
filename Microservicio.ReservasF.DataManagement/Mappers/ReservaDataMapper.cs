using Microservicio.ReservasF.DataAccess.Entities;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.DataManagement.Mappers;

public static class ReservaDataMapper
{
    // ============================================
    // ENTITY -> DATA MODEL
    // ============================================

    public static ReservaDataModel ToDataModel(
        ReservaEntity entity)
    {
        return new ReservaDataModel
        {
            IdReserva = entity.IdReserva,

            GuidReserva = entity.GuidReserva,

            CodigoReserva = entity.CodigoReserva,

            IdCliente = entity.IdCliente,

            IdVuelo = entity.IdVuelo,

            FechaReservaUtc = entity.FechaReservaUtc,

            FechaInicio = entity.FechaInicio,

            FechaFin = entity.FechaFin,

            SubtotalReserva = entity.SubtotalReserva,

            ValorIva = entity.ValorIva,

            TotalReserva = entity.TotalReserva,

            OrigenCanalReserva =
                entity.OrigenCanalReserva,

            EstadoReserva =
                entity.EstadoReserva,

            FechaConfirmacionUtc =
                entity.FechaConfirmacionUtc,

            FechaCancelacionUtc =
                entity.FechaCancelacionUtc,

            MotivoCancelacion =
                entity.MotivoCancelacion,

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

            ContactoEmail =
                entity.ContactoEmail,

            ContactoTelefono =
                entity.ContactoTelefono,

            Observaciones =
                entity.Observaciones,

            FechaInhabilitacionUtc =
                entity.FechaInhabilitacionUtc,

            MotivoInhabilitacion =
                entity.MotivoInhabilitacion,

            RowVersion =
                entity.RowVersion,

            Detalles = entity.Detalles
                .Where(d => !d.EsEliminado)
                .OrderBy(d => d.IdDetalle)
                .Select(ReservaDetalleDataMapper.ToDataModel)
                .ToList()
        };
    }

    // ============================================
    // DATA MODEL -> ENTITY
    // ============================================

    public static ReservaEntity ToEntity(
        ReservaDataModel model)
    {
        return new ReservaEntity
        {
            IdReserva = model.IdReserva,

            GuidReserva = model.GuidReserva,

            CodigoReserva = model.CodigoReserva,

            IdCliente = model.IdCliente,

            IdVuelo = model.IdVuelo,

            FechaReservaUtc = model.FechaReservaUtc,

            FechaInicio = model.FechaInicio,

            FechaFin = model.FechaFin,

            SubtotalReserva = model.SubtotalReserva,

            ValorIva = model.ValorIva,

            TotalReserva = model.TotalReserva,

            OrigenCanalReserva =
                model.OrigenCanalReserva,

            EstadoReserva =
                model.EstadoReserva,

            FechaConfirmacionUtc =
                model.FechaConfirmacionUtc,

            FechaCancelacionUtc =
                model.FechaCancelacionUtc,

            MotivoCancelacion =
                model.MotivoCancelacion,

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

            ContactoEmail =
                model.ContactoEmail,

            ContactoTelefono =
                model.ContactoTelefono,

            Observaciones =
                model.Observaciones,

            FechaInhabilitacionUtc =
                model.FechaInhabilitacionUtc,

            MotivoInhabilitacion =
                model.MotivoInhabilitacion,

            RowVersion =
                model.RowVersion,

            Detalles = model.Detalles
                .Select(ReservaDetalleDataMapper.ToEntity)
                .ToList()
        };
    }

    // ============================================
    // UPDATE ENTITY
    // ============================================

    public static void UpdateEntity(
        ReservaEntity entity,
        ReservaDataModel model)
    {
        entity.IdCliente =
            model.IdCliente;

        entity.IdVuelo =
            model.IdVuelo;

        entity.FechaInicio =
            model.FechaInicio;

        entity.FechaFin =
            model.FechaFin;

        entity.SubtotalReserva =
            model.SubtotalReserva;

        entity.ValorIva =
            model.ValorIva;

        entity.TotalReserva =
            model.TotalReserva;

        entity.OrigenCanalReserva =
            model.OrigenCanalReserva;

        entity.EstadoReserva =
            model.EstadoReserva;

        entity.FechaConfirmacionUtc =
            model.FechaConfirmacionUtc;

        entity.FechaCancelacionUtc =
            model.FechaCancelacionUtc;

        entity.MotivoCancelacion =
            model.MotivoCancelacion;

        entity.ContactoEmail =
            model.ContactoEmail;

        entity.ContactoTelefono =
            model.ContactoTelefono;

        entity.Observaciones =
            model.Observaciones;

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