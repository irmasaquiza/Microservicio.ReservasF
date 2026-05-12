
using Microservicio.ReservasF.DataAccess.Context;
using Microservicio.ReservasF.DataAccess.Entities;
using Microservicio.ReservasF.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.ReservasF.DataAccess.Repositories
{
    public class ReservaRepository
        : IReservaRepository
    {
        private readonly SistemaReservasDbContext _context;

        public ReservaRepository(
            SistemaReservasDbContext context)
        {
            _context = context;
        }

        // ============================================
        // CONSULTAS
        // ============================================

        public async Task<IReadOnlyCollection<ReservaEntity>>
            ObtenerTodosAsync(
                CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .Include(r => r.Detalles)
                .AsSplitQuery()
                .AsNoTracking()
                .Where(r => !r.EsEliminado)
                .OrderByDescending(r => r.FechaReservaUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<ReservaEntity?>
            ObtenerPorIdAsync(
                int idReserva,
                CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .Include(r => r.Detalles)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    r =>
                        r.IdReserva == idReserva &&
                        !r.EsEliminado,
                    cancellationToken);
        }

        public async Task<ReservaEntity?>
            ObtenerPorIdParaEditarAsync(
                int idReserva,
                CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .Include(r => r.Detalles)
                .AsSplitQuery()
                .FirstOrDefaultAsync(
                    r =>
                        r.IdReserva == idReserva &&
                        !r.EsEliminado,
                    cancellationToken);
        }

        public async Task<ReservaEntity?>
            ObtenerPorGuidAsync(
                Guid guidReserva,
                CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .Include(r => r.Detalles)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    r =>
                        r.GuidReserva == guidReserva &&
                        !r.EsEliminado,
                    cancellationToken);
        }

        public async Task<ReservaEntity?>
            ObtenerPorCodigoAsync(
                string codigoReserva,
                CancellationToken cancellationToken = default)
        {
            codigoReserva = codigoReserva
                .Trim()
                .ToUpperInvariant();

            return await _context.Reservas
                .Include(r => r.Detalles)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    r =>
                        r.CodigoReserva == codigoReserva &&
                        !r.EsEliminado,
                    cancellationToken);
        }

        public async Task<IReadOnlyCollection<ReservaEntity>>
            ObtenerPorClienteAsync(
                int idCliente,
                CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .Include(r => r.Detalles)
                .AsSplitQuery()
                .AsNoTracking()
                .Where(r =>
                    r.IdCliente == idCliente &&
                    !r.EsEliminado)
                .OrderByDescending(r => r.FechaReservaUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<ReservaEntity>>
            ObtenerPorPasajeroAsync(
                int idPasajero,
                CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .Include(r => r.Detalles)
                .AsSplitQuery()
                .AsNoTracking()
                .Where(r =>
                    !r.EsEliminado &&
                    r.Detalles.Any(d =>
                        !d.EsEliminado &&
                        d.IdPasajero == idPasajero))
                .OrderByDescending(r => r.FechaReservaUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<ReservaEntity>>
            ObtenerPorVueloAsync(
                int idVuelo,
                CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .Include(r => r.Detalles)
                .AsSplitQuery()
                .AsNoTracking()
                .Where(r =>
                    r.IdVuelo == idVuelo &&
                    !r.EsEliminado)
                .OrderByDescending(r => r.FechaReservaUtc)
                .ToListAsync(cancellationToken);
        }

        // ============================================
        // VALIDACIONES
        // ============================================

        public async Task<bool>
            ExistePorIdAsync(
                int idReserva,
                CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AnyAsync(
                    r =>
                        r.IdReserva == idReserva &&
                        !r.EsEliminado,
                    cancellationToken);
        }

        public async Task<bool>
            ExistePorGuidAsync(
                Guid guidReserva,
                CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AnyAsync(
                    r =>
                        r.GuidReserva == guidReserva &&
                        !r.EsEliminado,
                    cancellationToken);
        }

        public async Task<bool>
            ExistePorCodigoAsync(
                string codigoReserva,
                CancellationToken cancellationToken = default)
        {
            codigoReserva = codigoReserva
                .Trim()
                .ToUpperInvariant();

            return await _context.Reservas
                .AnyAsync(
                    r =>
                        r.CodigoReserva == codigoReserva &&
                        !r.EsEliminado,
                    cancellationToken);
        }

        public async Task<ReservaEntity?>
    ObtenerReservaActivaPorAsientoAsync(
        int idAsiento,
        CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .Include(r => r.Detalles)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    r =>
                        !r.EsEliminado &&
                        r.EstadoReserva != "CAN" &&
                        r.Detalles.Any(d =>
                            !d.EsEliminado &&
                            d.IdAsiento == idAsiento),
                    cancellationToken);
        }

        public async Task<ReservaEntity?>
            ObtenerPorVueloYAsientoAsync(
                int idVuelo,
                int idAsiento,
                CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .Include(r => r.Detalles)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    r =>
                        r.IdVuelo == idVuelo &&
                        !r.EsEliminado &&
                        r.Detalles.Any(d =>
                            !d.EsEliminado &&
                            d.IdAsiento == idAsiento),
                    cancellationToken);
        }

        public async Task<ReservaEntity?>
            ObtenerPorVueloYPasajeroAsync(
                int idVuelo,
                int idPasajero,
                CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .Include(r => r.Detalles)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    r =>
                        r.IdVuelo == idVuelo &&
                        !r.EsEliminado &&
                        r.Detalles.Any(d =>
                            !d.EsEliminado &&
                            d.IdPasajero == idPasajero),
                    cancellationToken);
        }

        public async Task<bool>
            ExistePorVueloYAsientoAsync(
                int idVuelo,
                int idAsiento,
                CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AnyAsync(
                    r =>
                        r.IdVuelo == idVuelo &&
                        !r.EsEliminado &&
                        r.EstadoReserva != "CAN" &&
                        r.Detalles.Any(d =>
                            !d.EsEliminado &&
                            d.IdAsiento == idAsiento),
                    cancellationToken);
        }

        public async Task<bool>
            ExistePorVueloYPasajeroAsync(
                int idVuelo,
                int idPasajero,
                CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AnyAsync(
                    r =>
                        r.IdVuelo == idVuelo &&
                        !r.EsEliminado &&
                        r.EstadoReserva != "CAN" &&
                        r.Detalles.Any(d =>
                            !d.EsEliminado &&
                            d.IdPasajero == idPasajero),
                    cancellationToken);
        }

        // ============================================
        // COMANDOS
        // ============================================

        public async Task AgregarAsync(
            ReservaEntity entity,
            CancellationToken cancellationToken = default)
        {
            await _context.Reservas.AddAsync(
                entity,
                cancellationToken);
        }

        public void Actualizar(
            ReservaEntity entity)
        {
            _context.Entry(entity).State =
                EntityState.Modified;
        }

        public void SoftDelete(
            ReservaEntity entity)
        {
            entity.EsEliminado = true;

            entity.FechaModificacionUtc =
                DateTime.UtcNow;

            _context.Entry(entity).State =
                EntityState.Modified;
        }
    }
}