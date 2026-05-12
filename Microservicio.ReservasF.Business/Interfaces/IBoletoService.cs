using Microservicio.ReservasF.Business.DTOs.Boleto;
using Microservicio.ReservasF.DataManagement.Common;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.Business.Interfaces;

public interface IBoletoService
{
    Task<DataPagedResult<BoletoResponseDto>> GetPagedAsync(BoletoFilterDto filter);

    Task<BoletoResponseDto?> GetByIdAsync(int idBoleto, int? idClienteDelToken, string rolDelToken);

    Task<BoletoResponseDto> CreateAsync(BoletoRequestDto request, string creadoPorUsuario);

    Task<BoletoResponseDto?> UpdateEstadoAsync(int idBoleto, BoletoUpdateRequestDto request, string modificadoPorUsuario);

    Task<bool> DeleteAsync(int idBoleto, string modificadoPorUsuario);
}