using Microservicio.ReservasF.Business.DTOs.Reserva;
using Microservicio.ReservasF.DataManagement.Common;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.Business.Interfaces;

public interface IReservaService
{
    Task<DataPagedResult<ReservaResponseDto>> GetPagedAsync(ReservaFilterDto filter);

    Task<ReservaResponseDto?> GetByIdAsync(
        int idReserva,
        int? idClienteDelToken,
        string rolDelToken);

    Task<ReservaResponseDto> CreateAsync(
        ReservaRequestDto request,
        string creadoPorUsuario);

    Task<ReservaResponseDto?> UpdateEstadoAsync(
        int idReserva,
        ReservaUpdateRequestDto request,
        string modificadoPorUsuario,
        int? idClienteDelToken,
        string rolDelToken);

    Task<ReservaPagarResponseDto?> PagarAsync(
        int idReserva,
        ReservaPagarRequestDto request,
        string modificadoPorUsuario,
        int? idClienteDelToken,
        string rolDelToken);

    Task<bool> DeleteAsync(
        int idReserva,
        string modificadoPorUsuario);
}