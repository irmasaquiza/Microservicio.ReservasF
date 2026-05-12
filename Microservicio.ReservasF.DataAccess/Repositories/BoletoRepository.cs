using Microservicio.ReservasF.DataAccess.Context;
using Microservicio.ReservasF.DataAccess.Entities;
using Microservicio.ReservasF.DataAccess.Repositories.Interfaces;
using Microservicio.ReservasF.DataAccess.Context;
using Microservicio.ReservasF.DataAccess.Entities;
using Microservicio.ReservasF.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.ReservasF.DataAccess.Repositories
{
    public class BoletoRepository : IBoletoRepository
    {
        private readonly SistemaReservasDbContext _context;

        public BoletoRepository(
            SistemaReservasDbContext context)
        {
            _context = context;
        }

        // ============================================
        // CONSULTAS
        // ============================================

        public async Task<IReadOnlyCollection<BoletoEntity>>
            ObtenerTodosAsync(
                CancellationToken cancellationToken = default)
        {
            return await _context.Boletos
                .AsNoTracking()
                .Where(b => !b.EsEliminado)
                .OrderByDescending(b => b.FechaEmision)
                .ToListAsync(cancellationToken);
        }

        public async Task<BoletoEntity?>
            ObtenerPorIdAsync(
                int idBoleto,
                CancellationToken cancellationToken = default)
        {
            return await _context.Boletos
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    b =>
                        b.IdBoleto == idBoleto &&
                        !b.EsEliminado,
                    cancellationToken);
        }

        public async Task<BoletoEntity?>
            ObtenerPorIdParaEditarAsync(
                int idBoleto,
                CancellationToken cancellationToken = default)
        {
            return await _context.Boletos
                .FirstOrDefaultAsync(
                    b =>
                        b.IdBoleto == idBoleto &&
                        !b.EsEliminado,
                    cancellationToken);
        }

        public async Task<BoletoEntity?>
            ObtenerPorCodigoAsync(
                string codigoBoleto,
                CancellationToken cancellationToken = default)
        {
            return await _context.Boletos
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    b =>
                        b.CodigoBoleto == codigoBoleto &&
                        !b.EsEliminado,
                    cancellationToken);
        }

        public async Task<IReadOnlyCollection<BoletoEntity>>
            ObtenerPorReservaAsync(
                int idReserva,
                CancellationToken cancellationToken = default)
        {
            return await _context.Boletos
                .AsNoTracking()
                .Where(b =>
                    b.IdReserva == idReserva &&
                    !b.EsEliminado)
                .OrderByDescending(b => b.FechaEmision)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<BoletoEntity>>
            ObtenerPorVueloAsync(
                int idVuelo,
                CancellationToken cancellationToken = default)
        {
            return await _context.Boletos
                .AsNoTracking()
                .Where(b =>
                    b.IdVuelo == idVuelo &&
                    !b.EsEliminado)
                .OrderByDescending(b => b.FechaEmision)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<BoletoEntity>>
            ObtenerPorAsientoAsync(
                int idAsiento,
                CancellationToken cancellationToken = default)
        {
            return await _context.Boletos
                .AsNoTracking()
                .Where(b =>
                    b.IdAsiento == idAsiento &&
                    !b.EsEliminado)
                .OrderByDescending(b => b.FechaEmision)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<BoletoEntity>>
            ObtenerPorFacturaAsync(
                int idFactura,
                CancellationToken cancellationToken = default)
        {
            return await _context.Boletos
                .AsNoTracking()
                .Where(b =>
                    b.IdFactura == idFactura &&
                    !b.EsEliminado)
                .OrderByDescending(b => b.FechaEmision)
                .ToListAsync(cancellationToken);
        }

        // ============================================
        // VALIDACIONES
        // ============================================

        public async Task<bool>
            ExistePorIdAsync(
                int idBoleto,
                CancellationToken cancellationToken = default)
        {
            return await _context.Boletos
                .AnyAsync(
                    b =>
                        b.IdBoleto == idBoleto &&
                        !b.EsEliminado,
                    cancellationToken);
        }

        public async Task<bool>
            ExistePorCodigoAsync(
                string codigoBoleto,
                CancellationToken cancellationToken = default)
        {
            return await _context.Boletos
                .AnyAsync(
                    b =>
                        b.CodigoBoleto == codigoBoleto &&
                        !b.EsEliminado,
                    cancellationToken);
        }

        // ============================================
        // COMANDOS
        // ============================================

        public async Task AgregarAsync(
            BoletoEntity entity,
            CancellationToken cancellationToken = default)
        {
            await _context.Boletos.AddAsync(
                entity,
                cancellationToken);
        }

        public void Actualizar(
            BoletoEntity entity)
        {
            _context.Entry(entity).State =
                EntityState.Modified;
        }

        public void SoftDelete(
            BoletoEntity entity)
        {
            entity.EsEliminado = true;

            entity.FechaModificacionUtc =
                DateTime.UtcNow;

            _context.Entry(entity).State =
                EntityState.Modified;
        }
    }
}