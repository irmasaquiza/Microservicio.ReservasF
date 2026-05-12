using Microservicio.ReservasF.Business.DTOs.Equipaje;
using Microservicio.ReservasF.DataManagement.Common;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.Business.Interfaces;

public interface IEquipajeService
{
    Task<DataPagedResult<EquipajeResponseDto>> GetPagedAsync(
        EquipajeFilterDto filter,
        int? idClienteDelToken,
        string rolDelToken);

    Task<EquipajeResponseDto?> GetByIdAsync(
        int idEquipaje,
        int? idClienteDelToken,
        string rolDelToken);

    Task<EquipajeResponseDto> CreateAsync(
        EquipajeRequestDto request,
        string creadoPorUsuario,
        int? idClienteDelToken,
        string rolDelToken);

    Task<EquipajeResponseDto?> UpdateEstadoAsync(
        int idEquipaje,
        EquipajeUpdateRequestDto request,
        string modificadoPorUsuario);

    Task<bool> DeleteAsync(
        int idEquipaje,
        string modificadoPorUsuario);
}