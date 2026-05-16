using System.Net;
using System.Net.Http.Json;
using Microservicio.ReservasF.Business.Integrations;
using Microservicio.ReservasF.Business.Integrations.Interfaces;


namespace Microservicio.ReservasF.Api.Integrations;



public class VueloIntegrationService : IVueloIntegrationService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public VueloIntegrationService(
        HttpClient httpClient,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;

        var baseUrl = _configuration["Integrations:Vuelos:BaseUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new InvalidOperationException("No está configurada la URL base del MS Vuelos.");

        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<VueloIntegrationDto?> ObtenerVueloAsync(
        int idVuelo,
        CancellationToken cancellationToken = default)
    {
        var token = _httpContextAccessor.HttpContext?
            .Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "");

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/v1/vuelos/{idVuelo}");

        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseIntegration<VueloIntegrationDto>>(
            cancellationToken: cancellationToken);

        return apiResponse?.Data;
    }

    public async Task<AsientoIntegrationDto?> ObtenerAsientoAsync(
        int idVuelo,
        int idAsiento,
        CancellationToken cancellationToken = default)
    {
        var token = _httpContextAccessor.HttpContext?
            .Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "");

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/v1/vuelos/{idVuelo}/asientos/{idAsiento}");

        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var apiResponse = await response.Content
            .ReadFromJsonAsync<ApiResponseIntegration<AsientoIntegrationDto>>(
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
        int idVuelo,
        int idAsiento,
        CancellationToken cancellationToken = default)
    {
        var asiento = await ObtenerAsientoAsync(idVuelo, idAsiento, cancellationToken);
        return asiento != null;
    }

    public async Task<bool> AsientoPerteneceAVueloAsync(
        int idAsiento,
        int idVuelo,
        CancellationToken cancellationToken = default)
    {
        var asiento = await ObtenerAsientoAsync(idVuelo, idAsiento, cancellationToken);
        return asiento != null && asiento.IdVuelo == idVuelo;
    }

    public async Task MarcarAsientoNoDisponibleAsync(
        int idVuelo,
        int idAsiento,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default)
    {
        var token = _httpContextAccessor.HttpContext?
            .Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "");

        var requestMessage = new HttpRequestMessage(
            HttpMethod.Patch,
            $"api/v1/vuelos/{idVuelo}/asientos/{idAsiento}");

        if (!string.IsNullOrEmpty(token))
            requestMessage.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        requestMessage.Content = JsonContent.Create(new { disponible = false });

        var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
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