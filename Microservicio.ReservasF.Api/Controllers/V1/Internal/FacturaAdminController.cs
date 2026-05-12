using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.ReservasF.Api.Models.Common;
using Microservicio.ReservasF.Business.DTOs.Factura;
using Microservicio.ReservasF.Business.Interfaces;

namespace Microservicio.ReservasF.Api.Controllers.V1.Internal;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/facturas")]
[Produces("application/json")]
[Authorize]
public class FacturaAdminController : ControllerBase
{
    private readonly IFacturaService _facturaService;

    public FacturaAdminController(IFacturaService facturaService)
    {
        _facturaService = facturaService;
    }

    [HttpGet]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetPaged([FromQuery] FacturaFilterDto filter)
    {
        var result = await _facturaService.GetPagedAsync(filter);

        return Ok(ApiResponse<object>.Ok(
            result,
            "Consulta de facturas realizada correctamente."));
    }

    [HttpGet("{id_factura:int}")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA,CLIENTE")]
    [ProducesResponseType(typeof(ApiResponse<FacturaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FacturaResponseDto>>> GetById(int id_factura)
    {
        var result = await _facturaService.GetByIdAsync(
            id_factura,
            GetIdCliente(),
            GetRol());

        if (result is null)
            return NotFound(ApiResponse<FacturaResponseDto>.Fail("Factura no encontrada."));

        return Ok(ApiResponse<FacturaResponseDto>.Ok(result));
    }

    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(typeof(ApiResponse<FacturaResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FacturaResponseDto>>> Create([FromBody] FacturaRequestDto request)
    {
        var usuario = GetUsuario();

        var result = await _facturaService.CreateAsync(request, usuario);

        return CreatedAtAction(
            nameof(GetById),
            new { id_factura = result.IdFactura, version = "1" },
            ApiResponse<FacturaResponseDto>.Ok(result, "Factura creada correctamente."));
    }

    [HttpPatch("{id_factura:int}/anular")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(typeof(ApiResponse<FacturaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<FacturaResponseDto>>> Anular(
        int id_factura,
        [FromBody] FacturaUpdateRequestDto? request)
    {
        request ??= new FacturaUpdateRequestDto();
        request.Estado = "INA";

        var usuario = GetUsuario();

        var result = await _facturaService.UpdateEstadoAsync(
            id_factura,
            request,
            usuario);

        if (result is null)
            return NotFound(ApiResponse<FacturaResponseDto>.Fail("Factura no encontrada."));

        return Ok(ApiResponse<FacturaResponseDto>.Ok(
            result,
            "Factura anulada correctamente."));
    }

    [HttpPatch("{id_factura:int}/aprobar")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(typeof(ApiResponse<FacturaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<FacturaResponseDto>>> Aprobar(int id_factura)
    {
        var usuario = GetUsuario();

        var result = await _facturaService.AprobarAsync(
            id_factura,
            usuario);

        if (result is null)
            return NotFound(ApiResponse<FacturaResponseDto>.Fail("Factura no encontrada."));

        return Ok(ApiResponse<FacturaResponseDto>.Ok(
            result,
            "Factura aprobada correctamente."));
    }

    [HttpPost("{id_factura:int}/pagar")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    [ProducesResponseType(typeof(ApiResponse<FacturaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<FacturaResponseDto>>> Pagar(int id_factura)
    {
        var usuario = GetUsuario();

        var result = await _facturaService.PagarAsync(
            id_factura,
            GetIdCliente(),
            GetRol(),
            usuario);

        if (result is null)
            return NotFound(ApiResponse<FacturaResponseDto>.Fail("Factura no encontrada."));

        return Ok(ApiResponse<FacturaResponseDto>.Ok(
            result,
            "Pago simulado correctamente. Factura aprobada."));
    }

    [HttpDelete("{id_factura:int}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id_factura)
    {
        var usuario = GetUsuario();

        var eliminado = await _facturaService.DeleteAsync(
            id_factura,
            usuario);

        if (!eliminado)
            return NotFound(ApiResponse<object>.Fail("Factura no encontrada."));

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