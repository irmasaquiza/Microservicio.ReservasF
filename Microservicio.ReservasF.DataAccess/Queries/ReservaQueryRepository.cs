using Microsoft.EntityFrameworkCore;

using Microservicio.ReservasF.DataAccess.Context;

namespace Microservicio.ReservasF.DataAccess.Queries
{
    public class ReservaQueryRepository
    {
        private readonly SistemaReservasDbContext _context;

        public ReservaQueryRepository(
            SistemaReservasDbContext context)
        {
            _context = context;
        }

        // ============================================
        // DTOs
        // ============================================

        public class ReservaCompletaDto
        {
            public int IdReserva { get; set; }

            public Guid GuidReserva { get; set; }

            public string CodigoReserva { get; set; }
                = string.Empty;

            public int IdCliente { get; set; }

            public int IdVuelo { get; set; }

            public string EstadoReserva { get; set; }
                = string.Empty;

            public DateTime FechaReservaUtc { get; set; }

            public DateTime FechaInicio { get; set; }

            public DateTime FechaFin { get; set; }

            public decimal SubtotalReserva { get; set; }

            public decimal ValorIva { get; set; }

            public decimal TotalReserva { get; set; }

            public List<ReservaDetalleDto> Detalles { get; set; }
                = new();

            public List<FacturaDto> Facturas { get; set; }
                = new();

            public List<BoletoDto> Boletos { get; set; }
                = new();
        }

        public class ReservaDetalleDto
        {
            public int IdDetalle { get; set; }

            public int IdPasajero { get; set; }

            public int IdAsiento { get; set; }
        }

        public class FacturaDto
        {
            public int IdFactura { get; set; }

            public string NumeroFactura { get; set; }
                = string.Empty;

            public decimal Total { get; set; }

            public string Estado { get; set; }
                = string.Empty;

            public DateTime FechaEmision { get; set; }
        }

        public class BoletoDto
        {
            public int IdBoleto { get; set; }

            public int IdVuelo { get; set; }

            public int IdAsiento { get; set; }

            public string CodigoBoleto { get; set; }
                = string.Empty;

            public decimal PrecioFinal { get; set; }

            public string EstadoBoleto { get; set; }
                = string.Empty;
        }

        // ============================================
        // DETALLE COMPLETO
        // ============================================

        public async Task<ReservaCompletaDto?>
            ObtenerDetalleCompletoPorIdAsync(
                int idReserva,
                CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AsNoTracking()
                .Where(r =>
                    r.IdReserva == idReserva &&
                    !r.EsEliminado)
                .Select(r => new ReservaCompletaDto
                {
                    IdReserva = r.IdReserva,

                    GuidReserva = r.GuidReserva,

                    CodigoReserva = r.CodigoReserva,

                    IdCliente = r.IdCliente,

                    IdVuelo = r.IdVuelo,

                    EstadoReserva = r.EstadoReserva,

                    FechaReservaUtc = r.FechaReservaUtc,

                    FechaInicio = r.FechaInicio,

                    FechaFin = r.FechaFin,

                    SubtotalReserva = r.SubtotalReserva,

                    ValorIva = r.ValorIva,

                    TotalReserva = r.TotalReserva,

                    Detalles = r.Detalles
                        .Where(d => !d.EsEliminado)
                        .Select(d => new ReservaDetalleDto
                        {
                            IdDetalle = d.IdDetalle,

                            IdPasajero = d.IdPasajero,

                            IdAsiento = d.IdAsiento
                        })
                        .ToList(),

                    Facturas = r.Facturas
                        .Where(f => !f.EsEliminado)
                        .Select(f => new FacturaDto
                        {
                            IdFactura = f.IdFactura,

                            NumeroFactura = f.NumeroFactura,

                            Total = f.Total,

                            Estado = f.Estado,

                            FechaEmision = f.FechaEmision
                        })
                        .ToList(),

                    Boletos = r.Boletos
                        .Where(b => !b.EsEliminado)
                        .Select(b => new BoletoDto
                        {
                            IdBoleto = b.IdBoleto,

                            IdVuelo = b.IdVuelo,

                            IdAsiento = b.IdAsiento,

                            CodigoBoleto = b.CodigoBoleto,

                            PrecioFinal = b.PrecioFinal,

                            EstadoBoleto = b.EstadoBoleto
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        // ============================================
        // RESUMEN
        // ============================================

        public class ReservaResumenDto
        {
            public int IdReserva { get; set; }

            public int IdCliente { get; set; }

            public string CodigoReserva { get; set; }
                = string.Empty;

            public string EstadoReserva { get; set; }
                = string.Empty;

            public decimal TotalReserva { get; set; }

            public DateTime FechaReservaUtc { get; set; }
        }

        public async Task<List<ReservaResumenDto>>
            ObtenerReservasPorClienteAsync(
                int idCliente,
                CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AsNoTracking()
                .Where(r =>
                    r.IdCliente == idCliente &&
                    !r.EsEliminado)
                .OrderByDescending(r => r.FechaReservaUtc)
                .Select(r => new ReservaResumenDto
                {
                    IdReserva = r.IdReserva,

                    IdCliente = r.IdCliente,

                    CodigoReserva = r.CodigoReserva,

                    EstadoReserva = r.EstadoReserva,

                    TotalReserva = r.TotalReserva,

                    FechaReservaUtc = r.FechaReservaUtc
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ReservaResumenDto>>
            ObtenerPendientesPorVencerAsync(
                DateTime fechaLimiteUtc,
                CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AsNoTracking()
                .Where(r =>
                    r.EstadoReserva == "PEN" &&
                    r.FechaInicio <= fechaLimiteUtc &&
                    !r.EsEliminado)
                .OrderBy(r => r.FechaInicio)
                .Select(r => new ReservaResumenDto
                {
                    IdReserva = r.IdReserva,

                    IdCliente = r.IdCliente,

                    CodigoReserva = r.CodigoReserva,

                    EstadoReserva = r.EstadoReserva,

                    TotalReserva = r.TotalReserva,

                    FechaReservaUtc = r.FechaReservaUtc
                })
                .ToListAsync(cancellationToken);
        }
    }
}