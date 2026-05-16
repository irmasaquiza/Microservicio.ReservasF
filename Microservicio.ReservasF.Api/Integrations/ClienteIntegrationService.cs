using System.Net;
using System.Net.Http.Json;
using Microservicio.ReservasF.Business.Integrations;
using Microservicio.ReservasF.Business.Integrations.Interfaces;

namespace Microservicio.ReservasF.Api.Integrations;

public class ClienteIntegrationService : IClienteIntegrationService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClienteIntegrationService(
        HttpClient httpClient,
        IConfiguration configuration,
    IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        var baseUrl = _configuration["Integrations:Clientes:BaseUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new InvalidOperationException("No está configurada la URL base del MS Clientes.");

        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<ClienteIntegrationDto?> GetClienteAsync(
        int idCliente,
        CancellationToken cancellationToken = default)
    {
        // ✅ Agregar el token del contexto HTTP
        var token = _httpContextAccessor.HttpContext?
            .Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "");

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/v1/clientes/{idCliente}");

        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseIntegration<ClienteIntegrationDto>>(
            cancellationToken: cancellationToken);

        return apiResponse?.Data;
    }

    public async Task<PasajeroIntegrationDto?> GetPasajeroAsync(
        int idPasajero,
        CancellationToken cancellationToken = default)
    {
        var token = _httpContextAccessor.HttpContext?
            .Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "");

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/v1/pasajeros/{idPasajero}");

        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseIntegration<PasajeroIntegrationDto>>(
            cancellationToken: cancellationToken);

        return apiResponse?.Data;
    }

    public async Task<bool> ExisteClienteAsync(
        int idCliente,
        CancellationToken cancellationToken = default)
    {
        var cliente = await GetClienteAsync(idCliente, cancellationToken);
        return cliente != null;
    }

    public async Task<bool> ClienteActivoAsync(
        int idCliente,
        CancellationToken cancellationToken = default)
    {
        var cliente = await GetClienteAsync(idCliente, cancellationToken);

        return cliente != null
            && !cliente.EsEliminado
            && cliente.Estado.Trim().ToUpperInvariant() == "ACT";
    }

    public async Task<bool> ExistePasajeroAsync(
        int idPasajero,
        CancellationToken cancellationToken = default)
    {
        var pasajero = await GetPasajeroAsync(idPasajero, cancellationToken);
        return pasajero != null;
    }

    public async Task<bool> PasajeroActivoAsync(
        int idPasajero,
        CancellationToken cancellationToken = default)
    {
        var pasajero = await GetPasajeroAsync(idPasajero, cancellationToken);

        return pasajero != null
            && !pasajero.EsEliminado
            && pasajero.Estado.Trim().ToUpperInvariant() == "ACTIVO";
    }

    public async Task<bool> PasajeroPerteneceAClienteAsync(
        int idPasajero,
        int idCliente,
        CancellationToken cancellationToken = default)
    {
        var pasajero = await GetPasajeroAsync(idPasajero, cancellationToken);

        if (pasajero == null)
            return false;

        return pasajero.IdCliente == idCliente;
    }

    private sealed class ApiResponseIntegration<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public IReadOnlyCollection<string> Errors { get; set; } = Array.Empty<string>();
    }
}