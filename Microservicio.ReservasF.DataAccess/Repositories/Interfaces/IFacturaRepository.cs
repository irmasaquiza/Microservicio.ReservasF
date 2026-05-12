using Microservicio.ReservasF.DataAccess.Entities;

namespace Microservicio.ReservasF.DataAccess.Repositories.Interfaces
{
    public interface IFacturaRepository
    {
        // ============================================
        // CONSULTAS
        // ============================================

        Task<IReadOnlyCollection<FacturaEntity>>
            ObtenerTodosAsync(
                CancellationToken cancellationToken = default);

        Task<FacturaEntity?>
            ObtenerPorIdAsync(
                int idFactura,
                CancellationToken cancellationToken = default);

        Task<FacturaEntity?>
            ObtenerPorIdParaEditarAsync(
                int idFactura,
                CancellationToken cancellationToken = default);

        Task<FacturaEntity?>
            ObtenerPorGuidAsync(
                Guid guidFactura,
                CancellationToken cancellationToken = default);

        Task<FacturaEntity?>
            ObtenerPorNumeroAsync(
                string numeroFactura,
                CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<FacturaEntity>>
            ObtenerPorClienteAsync(
                int idCliente,
                CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<FacturaEntity>>
            ObtenerPorReservaAsync(
                int idReserva,
                CancellationToken cancellationToken = default);

        // ============================================
        // VALIDACIONES
        // ============================================

        Task<bool>
            ExistePorIdAsync(
                int idFactura,
                CancellationToken cancellationToken = default);

        Task<bool>
            ExistePorGuidAsync(
                Guid guidFactura,
                CancellationToken cancellationToken = default);

        Task<bool>
            ExistePorNumeroAsync(
                string numeroFactura,
                CancellationToken cancellationToken = default);

        // ============================================
        // COMANDOS
        // ============================================

        Task AgregarAsync(
            FacturaEntity entity,
            CancellationToken cancellationToken = default);

        void Actualizar(
            FacturaEntity entity);

        void SoftDelete(
            FacturaEntity entity);
    }
}