using Microservicio.ReservasF.Business.DTOs.Factura;
using Microservicio.ReservasF.DataManagement.Common;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.Business.Interfaces;

public interface IFacturaService
{
    Task<DataPagedResult<FacturaResponseDto>> GetPagedAsync(FacturaFilterDto filter);

    Task<FacturaResponseDto?> GetByIdAsync(
        int idFactura,
        int? idClienteDelToken,
        string rolDelToken);

    Task<FacturaResponseDto> CreateAsync(
        FacturaRequestDto request,
        string creadoPorUsuario);

    Task<FacturaResponseDto?> UpdateEstadoAsync(
        int idFactura,
        FacturaUpdateRequestDto request,
        string modificadoPorUsuario);

    Task<FacturaResponseDto?> AprobarAsync(
        int idFactura,
        string modificadoPorUsuario);

    Task<FacturaResponseDto?> PagarAsync(
        int idFactura,
        int? idClienteDelToken,
        string rolDelToken,
        string modificadoPorUsuario);

    Task<bool> DeleteAsync(
        int idFactura,
        string modificadoPorUsuario);
}