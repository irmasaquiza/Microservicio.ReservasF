using Microservicio.ReservasF.DataAccess.Entities;

namespace Microservicio.ReservasF.DataAccess.Repositories.Interfaces
{
    public interface IReservaRepository
    {
        // ============================================
        // CONSULTAS
        // ============================================

        Task<IReadOnlyCollection<ReservaEntity>>
            ObtenerTodosAsync(
                CancellationToken cancellationToken = default);

        Task<ReservaEntity?>
            ObtenerPorIdAsync(
                int idReserva,
                CancellationToken cancellationToken = default);

        Task<ReservaEntity?>
            ObtenerPorIdParaEditarAsync(
                int idReserva,
                CancellationToken cancellationToken = default);

        Task<ReservaEntity?>
            ObtenerPorGuidAsync(
                Guid guidReserva,
                CancellationToken cancellationToken = default);

        Task<ReservaEntity?>
            ObtenerPorCodigoAsync(
                string codigoReserva,
                CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<ReservaEntity>>
            ObtenerPorClienteAsync(
                int idCliente,
                CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<ReservaEntity>>
            ObtenerPorPasajeroAsync(
                int idPasajero,
                CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<ReservaEntity>>
            ObtenerPorVueloAsync(
                int idVuelo,
                CancellationToken cancellationToken = default);

        Task<ReservaEntity?>
            ObtenerReservaActivaPorAsientoAsync(
                int idAsiento,
                CancellationToken cancellationToken = default);

        Task<ReservaEntity?>
            ObtenerPorVueloYAsientoAsync(
                int idVuelo,
                int idAsiento,
                CancellationToken cancellationToken = default);

        Task<ReservaEntity?>
            ObtenerPorVueloYPasajeroAsync(
                int idVuelo,
                int idPasajero,
                CancellationToken cancellationToken = default);

        // ============================================
        // VALIDACIONES
        // ============================================

        Task<bool>
            ExistePorIdAsync(
                int idReserva,
                CancellationToken cancellationToken = default);

        Task<bool>
            ExistePorGuidAsync(
                Guid guidReserva,
                CancellationToken cancellationToken = default);

        Task<bool>
            ExistePorCodigoAsync(
                string codigoReserva,
                CancellationToken cancellationToken = default);

        Task<bool>
            ExistePorVueloYAsientoAsync(
                int idVuelo,
                int idAsiento,
                CancellationToken cancellationToken = default);

        Task<bool>
            ExistePorVueloYPasajeroAsync(
                int idVuelo,
                int idPasajero,
                CancellationToken cancellationToken = default);

        // ============================================
        // COMANDOS
        // ============================================



        Task AgregarAsync(
            ReservaEntity entity,
            CancellationToken cancellationToken = default);

        void Actualizar(
            ReservaEntity entity);

        void SoftDelete(
            ReservaEntity entity);
    }
}