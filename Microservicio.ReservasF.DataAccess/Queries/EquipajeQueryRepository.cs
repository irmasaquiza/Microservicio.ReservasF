using Microsoft.EntityFrameworkCore;

using Microservicio.ReservasF.DataAccess.Context;

namespace Microservicio.ReservasF.DataAccess.Queries
{
    public class EquipajeQueryRepository
    {
        private readonly SistemaReservasDbContext _context;

        public EquipajeQueryRepository(
            SistemaReservasDbContext context)
        {
            _context = context;
        }

        public class EquipajeDetalleDto
        {
            public int IdEquipaje { get; set; }

            public int IdBoleto { get; set; }

            public int IdPasajero { get; set; }

            public int IdVuelo { get; set; }

            public string NumeroEtiqueta { get; set; }
                = string.Empty;

            public string Tipo { get; set; }
                = string.Empty;

            public decimal PesoKg { get; set; }

            public string EstadoEquipaje { get; set; }
                = string.Empty;
        }

        public async Task<List<EquipajeDetalleDto>>
            ObtenerPorBoletoConDetalleAsync(
                int idBoleto,
                CancellationToken cancellationToken = default)
        {
            return await _context.Equipajes
                .AsNoTracking()
                .Where(e =>
                    e.IdBoleto == idBoleto &&
                    !e.EsEliminado)
                .Select(e => new EquipajeDetalleDto
                {
                    IdEquipaje = e.IdEquipaje,

                    IdBoleto = e.IdBoleto,

                    IdPasajero = e.Boleto.Detalle.IdPasajero,

                    IdVuelo = e.Boleto.IdVuelo,

                    NumeroEtiqueta = e.NumeroEtiqueta,

                    Tipo = e.Tipo,

                    PesoKg = e.PesoKg,

                    EstadoEquipaje = e.EstadoEquipaje
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<EquipajeDetalleDto>>
            ObtenerEquipajeDeVueloAsync(
                int idVuelo,
                CancellationToken cancellationToken = default)
        {
            return await _context.Equipajes
                .AsNoTracking()
                .Where(e =>
                    e.Boleto.IdVuelo == idVuelo &&
                    !e.EsEliminado &&
                    !e.Boleto.EsEliminado)
                .Select(e => new EquipajeDetalleDto
                {
                    IdEquipaje = e.IdEquipaje,

                    IdBoleto = e.IdBoleto,

                    IdPasajero = e.Boleto.Detalle.IdPasajero,

                    IdVuelo = e.Boleto.IdVuelo,

                    NumeroEtiqueta = e.NumeroEtiqueta,

                    Tipo = e.Tipo,

                    PesoKg = e.PesoKg,

                    EstadoEquipaje = e.EstadoEquipaje
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<EquipajeDetalleDto>>
            ObtenerExtraviadoAsync(
                CancellationToken cancellationToken = default)
        {
            return await _context.Equipajes
                .AsNoTracking()
                .Where(e =>
                    e.EstadoEquipaje == "PERDIDO" &&
                    !e.EsEliminado)
                .Select(e => new EquipajeDetalleDto
                {
                    IdEquipaje = e.IdEquipaje,

                    IdBoleto = e.IdBoleto,

                    IdPasajero = e.Boleto.Detalle.IdPasajero,

                    IdVuelo = e.Boleto.IdVuelo,

                    NumeroEtiqueta = e.NumeroEtiqueta,

                    Tipo = e.Tipo,

                    PesoKg = e.PesoKg,

                    EstadoEquipaje = e.EstadoEquipaje
                })
                .ToListAsync(cancellationToken);
        }
    }
}