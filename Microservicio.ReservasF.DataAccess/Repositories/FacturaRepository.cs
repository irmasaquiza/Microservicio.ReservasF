
using Microservicio.ReservasF.DataAccess.Context;
using Microservicio.ReservasF.DataAccess.Entities;
using Microservicio.ReservasF.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.ReservasF.DataAccess.Repositories
{
    public class FacturaRepository
        : IFacturaRepository
    {
        private readonly SistemaReservasDbContext _context;

        public FacturaRepository(
            SistemaReservasDbContext context)
        {
            _context = context;
        }

        // ============================================
        // CONSULTAS
        // ============================================

        public async Task<IReadOnlyCollection<FacturaEntity>>
            ObtenerTodosAsync(
                CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AsNoTracking()
                .Where(f => !f.EsEliminado)
                .OrderByDescending(f => f.FechaEmision)
                .ToListAsync(cancellationToken);
        }

        public async Task<FacturaEntity?>
            ObtenerPorIdAsync(
                int idFactura,
                CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    f =>
                        f.IdFactura == idFactura &&
                        !f.EsEliminado,
                    cancellationToken);
        }

        public async Task<FacturaEntity?>
            ObtenerPorIdParaEditarAsync(
                int idFactura,
                CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .FirstOrDefaultAsync(
                    f =>
                        f.IdFactura == idFactura &&
                        !f.EsEliminado,
                    cancellationToken);
        }

        public async Task<FacturaEntity?>
            ObtenerPorGuidAsync(
                Guid guidFactura,
                CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    f =>
                        f.GuidFactura == guidFactura &&
                        !f.EsEliminado,
                    cancellationToken);
        }

        public async Task<FacturaEntity?>
            ObtenerPorNumeroAsync(
                string numeroFactura,
                CancellationToken cancellationToken = default)
        {
            numeroFactura = numeroFactura
                .Trim()
                .ToUpperInvariant();

            return await _context.Facturas
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    f =>
                        f.NumeroFactura == numeroFactura &&
                        !f.EsEliminado,
                    cancellationToken);
        }

        public async Task<IReadOnlyCollection<FacturaEntity>>
            ObtenerPorClienteAsync(
                int idCliente,
                CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AsNoTracking()
                .Where(f =>
                    f.IdCliente == idCliente &&
                    !f.EsEliminado)
                .OrderByDescending(f => f.FechaEmision)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<FacturaEntity>>
            ObtenerPorReservaAsync(
                int idReserva,
                CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AsNoTracking()
                .Where(f =>
                    f.IdReserva == idReserva &&
                    !f.EsEliminado)
                .OrderByDescending(f => f.FechaEmision)
                .ToListAsync(cancellationToken);
        }

        // ============================================
        // VALIDACIONES
        // ============================================

        public async Task<bool>
            ExistePorIdAsync(
                int idFactura,
                CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AnyAsync(
                    f =>
                        f.IdFactura == idFactura &&
                        !f.EsEliminado,
                    cancellationToken);
        }

        public async Task<bool>
            ExistePorGuidAsync(
                Guid guidFactura,
                CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AnyAsync(
                    f =>
                        f.GuidFactura == guidFactura &&
                        !f.EsEliminado,
                    cancellationToken);
        }

        public async Task<bool>
            ExistePorNumeroAsync(
                string numeroFactura,
                CancellationToken cancellationToken = default)
        {
            numeroFactura = numeroFactura
                .Trim()
                .ToUpperInvariant();

            return await _context.Facturas
                .AnyAsync(
                    f =>
                        f.NumeroFactura == numeroFactura &&
                        !f.EsEliminado,
                    cancellationToken);
        }

        // ============================================
        // COMANDOS
        // ============================================

        public async Task AgregarAsync(
            FacturaEntity entity,
            CancellationToken cancellationToken = default)
        {
            await _context.Facturas.AddAsync(
                entity,
                cancellationToken);
        }

        public void Actualizar(
            FacturaEntity entity)
        {
            _context.Entry(entity).State =
                EntityState.Modified;
        }

        public void SoftDelete(
            FacturaEntity entity)
        {
            entity.EsEliminado = true;

            entity.FechaModificacionUtc =
                DateTime.UtcNow;

            _context.Entry(entity).State =
                EntityState.Modified;
        }
    }
}