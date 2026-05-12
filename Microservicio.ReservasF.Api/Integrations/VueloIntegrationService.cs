using System.Net;
using System.Net.Http.Json;
using Microservicio.ReservasF.Business.Integrations;
using Microservicio.ReservasF.Business.Integrations.Interfaces;

namespace Microservicio.ReservasF.Api.Integrations;

public class VueloIntegrationService : IVueloIntegrationService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public VueloIntegrationService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;

        var baseUrl = _configuration["Integrations:Vuelos:BaseUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new InvalidOperationException("No está configurada la URL base del MS Vuelos.");

        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<VueloIntegrationDto?> ObtenerVueloAsync(
        int idVuelo,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"api/v1/vuelos/{idVuelo}",
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseIntegration<VueloIntegrationDto>>(
            cancellationToken: cancellationToken);

        return apiResponse?.Data;
    }

    public async Task<AsientoIntegrationDto?> ObtenerAsientoAsync(
        int idAsiento,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"api/v1/asientos/{idAsiento}",
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseIntegration<AsientoIntegrationDto>>(
            cancellationToken: cancellationToken);

        return apiResponse?.Data;
    }

    public async Task<bool> ExisteVueloAsync(
        int idVuelo,
        CancellationToken cancellationToken = default)
    {
        var vuelo = await ObtenerVueloAsync(idVuelo, cancellationToken);
        return vuelo != null;
    }

    public async Task<bool> VueloDisponibleParaReservaAsync(
        int idVuelo,
        CancellationToken cancellationToken = default)
    {
        var vuelo = await ObtenerVueloAsync(idVuelo, cancellationToken);

        if (vuelo == null)
            return false;

        var estado = vuelo.Estado.Trim().ToUpperInvariant();
        var estadoVuelo = vuelo.EstadoVuelo.Trim().ToUpperInvariant();

        return estado == "ACTIVO"
            && !vuelo.Eliminado
            && estadoVuelo is "PROGRAMADO" or "DEMORADO";
    }

    public async Task<bool> VueloPermiteEmisionAsync(
        int idVuelo,
        CancellationToken cancellationToken = default)
    {
        var vuelo = await ObtenerVueloAsync(idVuelo, cancellationToken);

        if (vuelo == null)
            return false;

        var estado = vuelo.Estado.Trim().ToUpperInvariant();
        var estadoVuelo = vuelo.EstadoVuelo.Trim().ToUpperInvariant();

        return estado == "ACTIVO"
            && !vuelo.Eliminado
            && estadoVuelo != "CANCELADO";
    }

    public async Task<bool> ExisteAsientoAsync(
        int idAsiento,
        CancellationToken cancellationToken = default)
    {
        var asiento = await ObtenerAsientoAsync(idAsiento, cancellationToken);
        return asiento != null;
    }

    public async Task<bool> AsientoPerteneceAVueloAsync(
        int idAsiento,
        int idVuelo,
        CancellationToken cancellationToken = default)
    {
        var asiento = await ObtenerAsientoAsync(idAsiento, cancellationToken);

        return asiento != null
            && asiento.IdVuelo == idVuelo;
    }

    public async Task MarcarAsientoNoDisponibleAsync(
        int idAsiento,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            disponible = false,
            modificado_por_usuario = modificadoPorUsuario
        };

        var response = await _httpClient.PatchAsJsonAsync(
            $"api/v1/asientos/{idAsiento}/disponibilidad",
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    private sealed class ApiResponseIntegration<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public IReadOnlyCollection<string> Errors { get; set; } = Array.Empty<string>();
    }
}