
using Microservicio.ReservasF.DataAccess.Context;
using Microservicio.ReservasF.DataAccess.Entities;
using Microservicio.ReservasF.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.ReservasF.DataAccess.Repositories
{
    public class EquipajeRepository
        : IEquipajeRepository
    {
        private readonly SistemaReservasDbContext _context;

        public EquipajeRepository(
            SistemaReservasDbContext context)
        {
            _context = context;
        }

        // ============================================
        // CONSULTAS
        // ============================================

        public async Task<IReadOnlyCollection<EquipajeEntity>>
            ObtenerTodosAsync(
                CancellationToken cancellationToken = default)
        {
            return await _context.Equipajes
                .AsNoTracking()
                .Where(e => !e.EsEliminado)
                .OrderByDescending(e => e.IdEquipaje)
                .ToListAsync(cancellationToken);
        }

        public async Task<EquipajeEntity?>
            ObtenerPorIdAsync(
                int idEquipaje,
                CancellationToken cancellationToken = default)
        {
            return await _context.Equipajes
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    e =>
                        e.IdEquipaje == idEquipaje &&
                        !e.EsEliminado,
                    cancellationToken);
        }

        public async Task<EquipajeEntity?>
            ObtenerPorIdParaEditarAsync(
                int idEquipaje,
                CancellationToken cancellationToken = default)
        {
            return await _context.Equipajes
                .FirstOrDefaultAsync(
                    e =>
                        e.IdEquipaje == idEquipaje &&
                        !e.EsEliminado,
                    cancellationToken);
        }

        public async Task<IReadOnlyCollection<EquipajeEntity>>
            ObtenerPorBoletoAsync(
                int idBoleto,
                CancellationToken cancellationToken = default)
        {
            return await _context.Equipajes
                .AsNoTracking()
                .Where(e =>
                    e.IdBoleto == idBoleto &&
                    !e.EsEliminado)
                .OrderByDescending(e => e.IdEquipaje)
                .ToListAsync(cancellationToken);
        }

        public async Task<EquipajeEntity?>
            ObtenerPorNumeroEtiquetaAsync(
                string numeroEtiqueta,
                CancellationToken cancellationToken = default)
        {
            return await _context.Equipajes
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    e =>
                        e.NumeroEtiqueta == numeroEtiqueta &&
                        !e.EsEliminado,
                    cancellationToken);
        }

        public async Task<IReadOnlyCollection<EquipajeEntity>>
            ObtenerPorTipoAsync(
                string tipo,
                CancellationToken cancellationToken = default)
        {
            tipo = tipo
                .Trim()
                .ToUpperInvariant();

            return await _context.Equipajes
                .AsNoTracking()
                .Where(e =>
                    e.Tipo == tipo &&
                    !e.EsEliminado)
                .OrderByDescending(e => e.IdEquipaje)
                .ToListAsync(cancellationToken);
        }

        // ============================================
        // VALIDACIONES
        // ============================================

        public async Task<bool>
            ExistePorIdAsync(
                int idEquipaje,
                CancellationToken cancellationToken = default)
        {
            return await _context.Equipajes
                .AnyAsync(
                    e =>
                        e.IdEquipaje == idEquipaje &&
                        !e.EsEliminado,
                    cancellationToken);
        }

        public async Task<bool>
            ExistePorNumeroEtiquetaAsync(
                string numeroEtiqueta,
                CancellationToken cancellationToken = default)
        {
            return await _context.Equipajes
                .AnyAsync(
                    e =>
                        e.NumeroEtiqueta == numeroEtiqueta &&
                        !e.EsEliminado,
                    cancellationToken);
        }

        // ============================================
        // COMANDOS
        // ============================================

        public async Task AgregarAsync(
            EquipajeEntity entity,
            CancellationToken cancellationToken = default)
        {
            await _context.Equipajes.AddAsync(
                entity,
                cancellationToken);
        }

        public void Actualizar(
            EquipajeEntity entity)
        {
            _context.Entry(entity).State =
                EntityState.Modified;
        }

        public void SoftDelete(
            EquipajeEntity entity)
        {
            entity.EsEliminado = true;

            entity.FechaModificacionUtc =
                DateTime.UtcNow;

            _context.Entry(entity).State =
                EntityState.Modified;
        }
    }
}