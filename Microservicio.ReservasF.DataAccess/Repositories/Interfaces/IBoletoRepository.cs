using Microservicio.ReservasF.DataAccess.Entities;

namespace Microservicio.ReservasF.DataAccess.Repositories.Interfaces
{
    public interface IBoletoRepository
    {
        // ============================================
        // CONSULTAS
        // ============================================

        Task<IReadOnlyCollection<BoletoEntity>>
            ObtenerTodosAsync(
                CancellationToken cancellationToken = default);

        Task<BoletoEntity?>
            ObtenerPorIdAsync(
                int idBoleto,
                CancellationToken cancellationToken = default);

        Task<BoletoEntity?>
            ObtenerPorIdParaEditarAsync(
                int idBoleto,
                CancellationToken cancellationToken = default);

        Task<BoletoEntity?>
            ObtenerPorCodigoAsync(
                string codigoBoleto,
                CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<BoletoEntity>>
            ObtenerPorReservaAsync(
                int idReserva,
                CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<BoletoEntity>>
            ObtenerPorVueloAsync(
                int idVuelo,
                CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<BoletoEntity>>
            ObtenerPorAsientoAsync(
                int idAsiento,
                CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<BoletoEntity>>
            ObtenerPorFacturaAsync(
                int idFactura,
                CancellationToken cancellationToken = default);

        // ============================================
        // VALIDACIONES
        // ============================================

        Task<bool>
            ExistePorIdAsync(
                int idBoleto,
                CancellationToken cancellationToken = default);

        Task<bool>
            ExistePorCodigoAsync(
                string codigoBoleto,
                CancellationToken cancellationToken = default);

        // ============================================
        // COMANDOS
        // ============================================

        Task AgregarAsync(
            BoletoEntity entity,
            CancellationToken cancellationToken = default);

        void Actualizar(
            BoletoEntity entity);

        void SoftDelete(
            BoletoEntity entity);
    }
}