using Microservicio.ReservasF.DataAccess.Entities;

namespace Microservicio.ReservasF.DataAccess.Repositories.Interfaces
{
    public interface IEquipajeRepository
    {
        // ============================================
        // CONSULTAS
        // ============================================

        Task<IReadOnlyCollection<EquipajeEntity>>
            ObtenerTodosAsync(
                CancellationToken cancellationToken = default);

        Task<EquipajeEntity?>
            ObtenerPorIdAsync(
                int idEquipaje,
                CancellationToken cancellationToken = default);

        Task<EquipajeEntity?>
            ObtenerPorIdParaEditarAsync(
                int idEquipaje,
                CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<EquipajeEntity>>
            ObtenerPorBoletoAsync(
                int idBoleto,
                CancellationToken cancellationToken = default);

        Task<EquipajeEntity?>
            ObtenerPorNumeroEtiquetaAsync(
                string numeroEtiqueta,
                CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<EquipajeEntity>>
            ObtenerPorTipoAsync(
                string tipo,
                CancellationToken cancellationToken = default);

        // ============================================
        // VALIDACIONES
        // ============================================

        Task<bool>
            ExistePorIdAsync(
                int idEquipaje,
                CancellationToken cancellationToken = default);

        Task<bool>
            ExistePorNumeroEtiquetaAsync(
                string numeroEtiqueta,
                CancellationToken cancellationToken = default);

        // ============================================
        // COMANDOS
        // ============================================

        Task AgregarAsync(
            EquipajeEntity entity,
            CancellationToken cancellationToken = default);

        void Actualizar(
            EquipajeEntity entity);

        void SoftDelete(
            EquipajeEntity entity);
    }
}