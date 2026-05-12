using Microservicio.ReservasF.DataManagement.Common;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.DataManagement.Interfaces;

public interface IFacturaDataService
{
    // ============================================
    // CONSULTAS
    // ============================================

    Task<DataPagedResult<FacturaDataModel>>
        GetPagedAsync(
            FacturaFiltroDataModel filtro);

    Task<FacturaDataModel?>
        GetByIdAsync(
            int id);

    Task<FacturaDataModel?>
        GetByGuidAsync(
            Guid guidFactura);

    Task<FacturaDataModel?>
        GetByNumeroAsync(
            string numeroFactura);

    Task<IReadOnlyCollection<FacturaDataModel>>
        GetByClienteAsync(
            int idCliente);

    Task<IReadOnlyCollection<FacturaDataModel>>
        GetByReservaAsync(
            int idReserva);

    // ============================================
    // VALIDACIONES
    // ============================================

    Task<bool>
        ExistePorIdAsync(
            int idFactura);

    Task<bool>
        ExistePorGuidAsync(
            Guid guidFactura);

    Task<bool>
        ExistePorNumeroAsync(
            string numeroFactura);

    // ============================================
    // COMANDOS
    // ============================================

    Task<FacturaDataModel>
        CreateAsync(
            FacturaDataModel model);

    Task<FacturaDataModel?>
        UpdateAsync(
            FacturaDataModel model);

    Task<bool>
        DeleteAsync(
            int id,
            string modificadoPorUsuario);
}