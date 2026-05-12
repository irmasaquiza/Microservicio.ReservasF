using Microsoft.EntityFrameworkCore;

using Microservicio.ReservasF.DataAccess.Context;

namespace Microservicio.ReservasF.DataAccess.Queries
{
    public class FacturaQueryRepository
    {
        private readonly SistemaReservasDbContext _context;

        public FacturaQueryRepository(
            SistemaReservasDbContext context)
        {
            _context = context;
        }

        public class FacturaDetalleDto
        {
            public int IdFactura { get; set; }

            public int IdCliente { get; set; }

            public int IdReserva { get; set; }

            public string NumeroFactura { get; set; }
                = string.Empty;

            public DateTime FechaEmision { get; set; }

            public decimal Subtotal { get; set; }

            public decimal ValorIva { get; set; }

            public decimal CargoServicio { get; set; }

            public decimal Total { get; set; }

            public string Estado { get; set; }
                = string.Empty;

            public string CodigoReserva { get; set; }
                = string.Empty;
        }

        public async Task<List<FacturaDetalleDto>>
            ObtenerPorReservaAsync(
                int idReserva,
                CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AsNoTracking()
                .Where(f =>
                    f.IdReserva == idReserva &&
                    !f.EsEliminado)
                .Select(f => new FacturaDetalleDto
                {
                    IdFactura = f.IdFactura,

                    IdCliente = f.IdCliente,

                    IdReserva = f.IdReserva,

                    NumeroFactura = f.NumeroFactura,

                    FechaEmision = f.FechaEmision,

                    Subtotal = f.Subtotal,

                    ValorIva = f.ValorIva,

                    CargoServicio = f.CargoServicio,

                    Total = f.Total,

                    Estado = f.Estado,

                    CodigoReserva = f.Reserva.CodigoReserva
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<FacturaDetalleDto>>
            ObtenerAprobadasPorRangoAsync(
                DateTime desde,
                DateTime hasta,
                CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AsNoTracking()
                .Where(f =>
                    f.Estado == "APR" &&
                    f.FechaEmision >= desde &&
                    f.FechaEmision <= hasta &&
                    !f.EsEliminado)
                .Select(f => new FacturaDetalleDto
                {
                    IdFactura = f.IdFactura,

                    IdCliente = f.IdCliente,

                    IdReserva = f.IdReserva,

                    NumeroFactura = f.NumeroFactura,

                    FechaEmision = f.FechaEmision,

                    Subtotal = f.Subtotal,

                    ValorIva = f.ValorIva,

                    CargoServicio = f.CargoServicio,

                    Total = f.Total,

                    Estado = f.Estado,

                    CodigoReserva = f.Reserva.CodigoReserva
                })
                .ToListAsync(cancellationToken);
        }

        public class ResumenFacturasEstadoDto
        {
            public string Estado { get; set; }
                = string.Empty;

            public int Cantidad { get; set; }

            public decimal TotalAcumulado { get; set; }
        }

        public async Task<List<ResumenFacturasEstadoDto>>
            ObtenerResumenPorEstadoAsync(
                CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AsNoTracking()
                .Where(f => !f.EsEliminado)
                .GroupBy(f => f.Estado)
                .Select(g => new ResumenFacturasEstadoDto
                {
                    Estado = g.Key,

                    Cantidad = g.Count(),

                    TotalAcumulado = g.Sum(x => x.Total)
                })
                .ToListAsync(cancellationToken);
        }
    }
}