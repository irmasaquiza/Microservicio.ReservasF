using Microservicio.ReservasF.DataManagement.Common;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.DataManagement.Interfaces;

public interface IEquipajeDataService
{
    // ============================================
    // CONSULTAS
    // ============================================

    Task<DataPagedResult<EquipajeDataModel>>
        GetPagedAsync(
            EquipajeFiltroDataModel filtro);

    Task<EquipajeDataModel?>
        GetByIdAsync(
            int id);

    Task<EquipajeDataModel?>
        GetByNumeroEtiquetaAsync(
            string numeroEtiqueta);

    Task<IReadOnlyCollection<EquipajeDataModel>>
        GetByBoletoAsync(
            int idBoleto);

    Task<IReadOnlyCollection<EquipajeDataModel>>
        GetByTipoAsync(
            string tipo);

    // ============================================
    // VALIDACIONES
    // ============================================

    Task<bool>
        ExistePorIdAsync(
            int idEquipaje);

    Task<bool>
        ExistePorNumeroEtiquetaAsync(
            string numeroEtiqueta);

    // ============================================
    // COMANDOS
    // ============================================

    Task<EquipajeDataModel>
        CreateAsync(
            EquipajeDataModel model);

    Task<EquipajeDataModel?>
        UpdateAsync(
            EquipajeDataModel model);

    Task<bool>
        DeleteAsync(
            int id,
            string modificadoPorUsuario);
}