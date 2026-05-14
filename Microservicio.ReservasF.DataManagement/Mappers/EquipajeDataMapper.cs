using Microservicio.ReservasF.DataAccess.Entities;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.DataManagement.Mappers;

public static class EquipajeDataMapper
{
    // ============================================
    // ENTITY -> DATA MODEL
    // ============================================

    public static EquipajeDataModel ToDataModel(
        EquipajeEntity entity)
    {
        return new EquipajeDataModel
        {
            IdEquipaje = entity.IdEquipaje,

            IdBoleto = entity.IdBoleto,

            Tipo = entity.Tipo,
            PesoKg = entity.PesoKg,

            DescripcionEquipaje =
                entity.DescripcionEquipaje,

            PrecioExtra = entity.PrecioExtra,

            DimensionesCm =
                entity.DimensionesCm,

            NumeroEtiqueta =
                entity.NumeroEtiqueta,

            EstadoEquipaje =
                entity.EstadoEquipaje,

            EsEliminado =
                entity.EsEliminado,

            Estado =
                entity.Estado,

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

    public static EquipajeEntity ToEntity(
        EquipajeDataModel model)
    {
        return new EquipajeEntity
        {
            IdEquipaje = model.IdEquipaje,

            IdBoleto = model.IdBoleto,

            Tipo = model.Tipo,
            PesoKg = model.PesoKg,

            DescripcionEquipaje =
                model.DescripcionEquipaje,

            PrecioExtra = model.PrecioExtra,

            DimensionesCm =
                model.DimensionesCm,

            NumeroEtiqueta =
                model.NumeroEtiqueta,

            EstadoEquipaje =
                model.EstadoEquipaje,

            EsEliminado =
                model.EsEliminado,

            Estado =
                model.Estado,

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
        EquipajeEntity entity,
        EquipajeDataModel model)
    {
        entity.IdBoleto =
            model.IdBoleto;

        entity.Tipo =
            model.Tipo;

        entity.PesoKg =
            model.PesoKg;

        entity.DescripcionEquipaje =
            model.DescripcionEquipaje;

        entity.PrecioExtra =
            model.PrecioExtra;

        entity.DimensionesCm =
            model.DimensionesCm;

        entity.NumeroEtiqueta =
            model.NumeroEtiqueta;

        entity.EstadoEquipaje =
            model.EstadoEquipaje;

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