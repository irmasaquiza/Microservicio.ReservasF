using Microservicio.ReservasF.DataAccess.Entities;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.DataManagement.Mappers;

public static class BoletoDataMapper
{
    // ============================================
    // ENTITY -> DATA MODEL
    // ============================================

    public static BoletoDataModel ToDataModel(
        BoletoEntity entity)
    {
        return new BoletoDataModel
        {
            IdBoleto = entity.IdBoleto,
            RowVersion = entity.RowVersion,

            IdReserva = entity.IdReserva,
            IdDetalle = entity.IdDetalle,
            IdVuelo = entity.IdVuelo,
            IdAsiento = entity.IdAsiento,
            IdFactura = entity.IdFactura,

            CodigoBoleto = entity.CodigoBoleto,
            Clase = entity.Clase,

            PrecioVueloBase = entity.PrecioVueloBase,
            PrecioAsientoExtra = entity.PrecioAsientoExtra,
            ImpuestosBoleto = entity.ImpuestosBoleto,
            CargoEquipaje = entity.CargoEquipaje,
            PrecioFinal = entity.PrecioFinal,

            EstadoBoleto = entity.EstadoBoleto,

            FechaEmision = entity.FechaEmision,

            EsEliminado = entity.EsEliminado,
            Estado = entity.Estado,

            CreadoPorUsuario = entity.CreadoPorUsuario,
            FechaRegistroUtc = entity.FechaRegistroUtc,

            ModificadoPorUsuario = entity.ModificadoPorUsuario,
            FechaModificacionUtc = entity.FechaModificacionUtc,
            ModificacionIp = entity.ModificacionIp
        };
    }

    // ============================================
    // DATA MODEL -> ENTITY
    // ============================================

    public static BoletoEntity ToEntity(
        BoletoDataModel model)
    {
        return new BoletoEntity
        {
            IdBoleto = model.IdBoleto,
            RowVersion = model.RowVersion,

            IdReserva = model.IdReserva,
            IdDetalle = model.IdDetalle,
            IdVuelo = model.IdVuelo,
            IdAsiento = model.IdAsiento,
            IdFactura = model.IdFactura,

            CodigoBoleto = model.CodigoBoleto,
            Clase = model.Clase,

            PrecioVueloBase = model.PrecioVueloBase,
            PrecioAsientoExtra = model.PrecioAsientoExtra,
            ImpuestosBoleto = model.ImpuestosBoleto,
            CargoEquipaje = model.CargoEquipaje,
            PrecioFinal = model.PrecioFinal,

            EstadoBoleto = model.EstadoBoleto,

            FechaEmision = model.FechaEmision,

            EsEliminado = model.EsEliminado,
            Estado = model.Estado,

            CreadoPorUsuario = model.CreadoPorUsuario,
            FechaRegistroUtc = model.FechaRegistroUtc,

            ModificadoPorUsuario = model.ModificadoPorUsuario,
            FechaModificacionUtc = model.FechaModificacionUtc,
            ModificacionIp = model.ModificacionIp
        };
    }

    // ============================================
    // UPDATE ENTITY
    // ============================================

    public static void UpdateEntity(
        BoletoEntity entity,
        BoletoDataModel model)
    {
        entity.IdReserva = model.IdReserva;
        entity.IdDetalle = model.IdDetalle;
        entity.IdVuelo = model.IdVuelo;
        entity.IdAsiento = model.IdAsiento;
        entity.IdFactura = model.IdFactura;

        entity.CodigoBoleto = model.CodigoBoleto;
        entity.Clase = model.Clase;

        entity.PrecioVueloBase = model.PrecioVueloBase;
        entity.PrecioAsientoExtra = model.PrecioAsientoExtra;
        entity.ImpuestosBoleto = model.ImpuestosBoleto;
        entity.CargoEquipaje = model.CargoEquipaje;
        entity.PrecioFinal = model.PrecioFinal;

        entity.EstadoBoleto = model.EstadoBoleto;

        entity.FechaEmision = model.FechaEmision;

        entity.Estado = model.Estado;

        entity.ModificadoPorUsuario =
            model.ModificadoPorUsuario;

        entity.FechaModificacionUtc =
            model.FechaModificacionUtc;

        entity.ModificacionIp =
            model.ModificacionIp;
    }
}