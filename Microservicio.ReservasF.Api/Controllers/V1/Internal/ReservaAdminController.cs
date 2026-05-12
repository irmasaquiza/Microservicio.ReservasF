using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.ReservasF.Api.Models.Common;
using Microservicio.ReservasF.Business.DTOs.Reserva;
using Microservicio.ReservasF.Business.Interfaces;

namespace Microservicio.ReservasF.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/reservas")]
[Produces("application/json")]
[Authorize]
public class ReservaAdminController : ControllerBase
{
    private readonly IReservaService _reservaService;

    public ReservaAdminController(IReservaService reservaService)
    {
        _reservaService = reservaService;
    }

    [HttpGet]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged([FromQuery] ReservaFilterDto filter)
    {
        var result = await _reservaService.GetPagedAsync(filter);

        return Ok(ApiResponse<object>.Ok(
            result,
            "Consulta de reservas realizada correctamente."));
    }

    [HttpGet("{id_reserva:int}")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
    [ProducesResponseType(typeof(ApiResponse<ReservaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ReservaResponseDto>>> GetById(int id_reserva)
    {
        var result = await _reservaService.GetByIdAsync(
            id_reserva,
            GetIdCliente(),
            GetRol());

        if (result is null)
            return NotFound(ApiResponse<ReservaResponseDto>.Fail("Reserva no encontrada."));

        return Ok(ApiResponse<ReservaResponseDto>.Ok(result));
    }

    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
    [ProducesResponseType(typeof(ApiResponse<ReservaResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<ReservaResponseDto>>> Create([FromBody] ReservaRequestDto request)
    {
        if (GetRol() == "CLIENTE")
        {
            var idCliente = GetIdCliente();

            if (idCliente is null)
                return Unauthorized(ApiResponse<ReservaResponseDto>.Fail("No se pudo identificar el cliente del token."));

            request.IdCliente = idCliente.Value;
        }

        var result = await _reservaService.CreateAsync(
            request,
            GetUsuario());

        return CreatedAtAction(
            nameof(GetById),
            new { id_reserva = result.IdReserva, version = "1" },
            ApiResponse<ReservaResponseDto>.Ok(result, "Reserva creada correctamente."));
    }

    [HttpPatch("{id_reserva:int}/estado")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
    [ProducesResponseType(typeof(ApiResponse<ReservaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ReservaResponseDto>>> UpdateEstado(
        int id_reserva,
        [FromBody] ReservaUpdateRequestDto request)
    {
        var result = await _reservaService.UpdateEstadoAsync(
            id_reserva,
            request,
            GetUsuario(),
            GetIdCliente(),
            GetRol());

        if (result is null)
            return NotFound(ApiResponse<ReservaResponseDto>.Fail("Reserva no encontrada."));

        return Ok(ApiResponse<ReservaResponseDto>.Ok(
            result,
            "Estado de reserva actualizado correctamente."));
    }

    [HttpPatch("{id_reserva:int}/pagar")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
    [ProducesResponseType(typeof(ApiResponse<ReservaPagarResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ReservaPagarResponseDto>>> Pagar(
        int id_reserva,
        [FromBody] ReservaPagarRequestDto request)
    {
        var result = await _reservaService.PagarAsync(
            id_reserva,
            request,
            GetUsuario(),
            GetIdCliente(),
            GetRol());

        if (result is null)
            return NotFound(ApiResponse<ReservaPagarResponseDto>.Fail("Reserva no encontrada."));

        return Ok(ApiResponse<ReservaPagarResponseDto>.Ok(
            result,
            "Pago de reserva procesado correctamente."));
    }

    [HttpDelete("{id_reserva:int}")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id_reserva)
    {
        var eliminado = await _reservaService.DeleteAsync(
            id_reserva,
            GetUsuario());

        if (!eliminado)
            return NotFound(ApiResponse<object>.Fail("Reserva no encontrada."));

        return NoContent();
    }

    private string GetUsuario()
    {
        var name = User?.Identity?.Name;

        if (!string.IsNullOrWhiteSpace(name))
            return name.Trim();

        var username = User?.FindFirst("username")?.Value;

        if (!string.IsNullOrWhiteSpace(username))
            return username.Trim();

        return "SYSTEM";
    }

    private int? GetIdCliente()
    {
        var claim = User.FindFirst("id_cliente")?.Value;

        return int.TryParse(claim, out var id)
            ? id
            : null;
    }

    private string GetRol()
    {
        return User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
            ?? User.FindFirst("role")?.Value
            ?? string.Empty;
    }
}