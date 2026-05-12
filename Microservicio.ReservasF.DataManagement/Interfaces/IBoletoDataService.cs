using Microservicio.ReservasF.DataManagement.Common;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.DataManagement.Interfaces;

public interface IBoletoDataService
{
    // ============================================
    // CONSULTAS
    // ============================================

    Task<DataPagedResult<BoletoDataModel>>
        GetPagedAsync(
            BoletoFiltroDataModel filtro);

    Task<BoletoDataModel?>
        GetByIdAsync(
            int id);

    Task<BoletoDataModel?>
        GetByCodigoAsync(
            string codigoBoleto);

    Task<IReadOnlyCollection<BoletoDataModel>>
        GetByReservaAsync(
            int idReserva);

    Task<IReadOnlyCollection<BoletoDataModel>>
        GetByVueloAsync(
            int idVuelo);

    Task<IReadOnlyCollection<BoletoDataModel>>
        GetByFacturaAsync(
            int idFactura);

    // ============================================
    // VALIDACIONES
    // ============================================

    Task<bool>
        ExistePorIdAsync(
            int idBoleto);

    Task<bool>
        ExistePorCodigoAsync(
            string codigoBoleto);

    // ============================================
    // COMANDOS
    // ============================================

    Task<BoletoDataModel>
        CreateAsync(
            BoletoDataModel model);

    Task<BoletoDataModel?>
        UpdateAsync(
            BoletoDataModel model);

    Task<bool>
        DeleteAsync(
            int id,
            string modificadoPorUsuario);
}