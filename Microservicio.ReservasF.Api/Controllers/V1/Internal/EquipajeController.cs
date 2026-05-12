using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.ReservasF.Api.Models.Common;
using Microservicio.ReservasF.Business.DTOs.Equipaje;
using Microservicio.ReservasF.Business.Interfaces;

namespace Microservicio.ReservasF.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/boletos/{id_boleto:int}/equipaje")]
[Produces("application/json")]
[Authorize]
public class EquipajeController : ControllerBase
{
    private readonly IEquipajeService _equipajeService;

    public EquipajeController(IEquipajeService equipajeService)
    {
        _equipajeService = equipajeService;
    }

    [HttpGet]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged(
        int id_boleto,
        [FromQuery] EquipajeFilterDto filter)
    {
        filter.IdBoleto = id_boleto;

        var result = await _equipajeService.GetPagedAsync(
            filter,
            GetIdCliente(),
            GetRol());

        return Ok(ApiResponse<object>.Ok(
            result,
            "Consulta de equipajes realizada correctamente."));
    }

    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
    [ProducesResponseType(typeof(ApiResponse<EquipajeResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<EquipajeResponseDto>>> Create(
        int id_boleto,
        [FromBody] EquipajeRequestDto request)
    {
        request.IdBoleto = id_boleto;

        var usuario = GetUsuario();

        var result = await _equipajeService.CreateAsync(
            request,
            usuario,
            GetIdCliente(),
            GetRol());

        return CreatedAtAction(
            nameof(GetPaged),
            new { id_boleto = result.IdBoleto, version = "1" },
            ApiResponse<EquipajeResponseDto>.Ok(result, "Equipaje registrado correctamente."));
    }

    [HttpPatch("{id_equipaje:int}/estado")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(typeof(ApiResponse<EquipajeResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<EquipajeResponseDto>>> UpdateEstado(
        int id_boleto,
        int id_equipaje,
        [FromBody] EquipajeUpdateRequestDto request)
    {
        var existente = await _equipajeService.GetByIdAsync(
            id_equipaje,
            GetIdCliente(),
            GetRol());

        if (existente is null || existente.IdBoleto != id_boleto)
            return NotFound(ApiResponse<EquipajeResponseDto>.Fail("Equipaje no encontrado."));

        var usuario = GetUsuario();

        var result = await _equipajeService.UpdateEstadoAsync(
            id_equipaje,
            request,
            usuario);

        if (result is null)
            return NotFound(ApiResponse<EquipajeResponseDto>.Fail("Equipaje no encontrado."));

        return Ok(ApiResponse<EquipajeResponseDto>.Ok(
            result,
            "Estado del equipaje actualizado correctamente."));
    }

    [HttpDelete("{id_equipaje:int}")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id_boleto,
        int id_equipaje)
    {
        var existente = await _equipajeService.GetByIdAsync(
            id_equipaje,
            GetIdCliente(),
            GetRol());

        if (existente is null || existente.IdBoleto != id_boleto)
            return NotFound(ApiResponse<object>.Fail("Equipaje no encontrado."));

        var usuario = GetUsuario();

        var eliminado = await _equipajeService.DeleteAsync(
            id_equipaje,
            usuario);

        if (!eliminado)
            return NotFound(ApiResponse<object>.Fail("Equipaje no encontrado."));

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