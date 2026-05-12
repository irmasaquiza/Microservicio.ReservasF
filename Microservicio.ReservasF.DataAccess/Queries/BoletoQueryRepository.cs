using Microsoft.EntityFrameworkCore;

using Microservicio.ReservasF.DataAccess.Context;

namespace Microservicio.ReservasF.DataAccess.Queries
{
    public class BoletoQueryRepository
    {
        private readonly SistemaReservasDbContext _context;

        public BoletoQueryRepository(
            SistemaReservasDbContext context)
        {
            _context = context;
        }

        public class EquipajeDto
        {
            public int IdEquipaje { get; set; }

            public string Tipo { get; set; } = string.Empty;

            public decimal PesoKg { get; set; }

            public string EstadoEquipaje { get; set; } = string.Empty;
        }

        public class BoletoDetalleDto
        {
            public int IdBoleto { get; set; }

            public int IdPasajero { get; set; }

            public int IdVuelo { get; set; }

            public int IdAsiento { get; set; }

            public string CodigoBoleto { get; set; }
                = string.Empty;

            public decimal PrecioFinal { get; set; }

            public List<EquipajeDto> Equipajes { get; set; }
                = new();
        }

        public async Task<BoletoDetalleDto?>
            ObtenerDetalleCompletoAsync(
                int idBoleto,
                CancellationToken cancellationToken = default)
        {
            return await _context.Boletos
                .AsNoTracking()
                .Where(b =>
                    b.IdBoleto == idBoleto &&
                    !b.EsEliminado)
                .Select(b => new BoletoDetalleDto
                {
                    IdBoleto = b.IdBoleto,

                    IdPasajero = b.Detalle.IdPasajero,

                    IdVuelo = b.IdVuelo,

                    IdAsiento = b.IdAsiento,

                    CodigoBoleto = b.CodigoBoleto,

                    PrecioFinal = b.PrecioFinal,

                    Equipajes = b.Equipajes
                        .Where(e => !e.EsEliminado)
                        .Select(e => new EquipajeDto
                        {
                            IdEquipaje = e.IdEquipaje,

                            Tipo = e.Tipo,

                            PesoKg = e.PesoKg,

                            EstadoEquipaje = e.EstadoEquipaje
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<BoletoDetalleDto>>
            ObtenerPorReservaAsync(
                int idReserva,
                CancellationToken cancellationToken = default)
        {
            return await _context.Boletos
                .AsNoTracking()
                .Where(b =>
                    b.IdReserva == idReserva &&
                    !b.EsEliminado)
                .Select(b => new BoletoDetalleDto
                {
                    IdBoleto = b.IdBoleto,

                    IdPasajero = b.Detalle.IdPasajero,

                    IdVuelo = b.IdVuelo,

                    IdAsiento = b.IdAsiento,

                    CodigoBoleto = b.CodigoBoleto,

                    PrecioFinal = b.PrecioFinal,

                    Equipajes = b.Equipajes
                        .Where(e => !e.EsEliminado)
                        .Select(e => new EquipajeDto
                        {
                            IdEquipaje = e.IdEquipaje,

                            Tipo = e.Tipo,

                            PesoKg = e.PesoKg,

                            EstadoEquipaje = e.EstadoEquipaje
                        })
                        .ToList()
                })
                .ToListAsync(cancellationToken);
        }

        public class ManifiestoPasajeroDto
        {
            public int IdPasajero { get; set; }

            public int IdAsiento { get; set; }

            public string CodigoBoleto { get; set; }
                = string.Empty;
        }

        public async Task<List<ManifiestoPasajeroDto>>
            ObtenerManifiestoPorVueloAsync(
                int idVuelo,
                CancellationToken cancellationToken = default)
        {
            return await _context.Boletos
                .AsNoTracking()
                .Where(b =>
                    b.IdVuelo == idVuelo &&
                    !b.EsEliminado)
                .Select(b => new ManifiestoPasajeroDto
                {
                    IdPasajero = b.Detalle.IdPasajero,

                    IdAsiento = b.IdAsiento,

                    CodigoBoleto = b.CodigoBoleto
                })
                .ToListAsync(cancellationToken);
        }
    }
}