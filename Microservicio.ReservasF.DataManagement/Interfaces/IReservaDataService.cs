using Microservicio.ReservasF.DataManagement.Common;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.DataManagement.Interfaces;

public interface IReservaDataService
{
    // ============================================
    // CONSULTAS
    // ============================================

    Task<DataPagedResult<ReservaDataModel>>
        GetPagedAsync(
            ReservaFiltroDataModel filtro);

    Task<ReservaDataModel?>
        GetByIdAsync(
            int id);

    Task<ReservaDataModel?>
        GetByGuidAsync(
            Guid guidReserva);

    Task<ReservaDataModel?>
        GetByCodigoAsync(
            string codigoReserva);

    Task<IReadOnlyCollection<ReservaDataModel>>
        GetByClienteAsync(
            int idCliente);

    Task<IReadOnlyCollection<ReservaDataModel>>
        GetByPasajeroAsync(
            int idPasajero);

    Task<IReadOnlyCollection<ReservaDataModel>>
        GetByVueloAsync(
            int idVuelo);

    Task<ReservaDataModel?>
        GetReservaActivaPorAsientoAsync(
            int idAsiento);

    Task<ReservaDataModel?>
        GetPorVueloYAsientoAsync(
            int idVuelo,
            int idAsiento);

    Task<ReservaDataModel?>
        GetPorVueloYPasajeroAsync(
            int idVuelo,
            int idPasajero);

    // ============================================
    // VALIDACIONES
    // ============================================

    Task<bool>
        ExistePorIdAsync(
            int idReserva);

    Task<bool>
        ExistePorGuidAsync(
            Guid guidReserva);

    Task<bool>
        ExistePorCodigoAsync(
            string codigoReserva);

    Task<bool>
        ExistePorVueloYAsientoAsync(
            int idVuelo,
            int idAsiento);

    Task<bool>
        ExistePorVueloYPasajeroAsync(
            int idVuelo,
            int idPasajero);

    // ============================================
    // COMANDOS
    // ============================================

    Task<ReservaDataModel>
        CreateAsync(
            ReservaDataModel model);

    Task<ReservaDataModel?>
        UpdateAsync(
            ReservaDataModel model);

    Task<bool>
        DeleteAsync(
            int id,
            string modificadoPorUsuario);
}