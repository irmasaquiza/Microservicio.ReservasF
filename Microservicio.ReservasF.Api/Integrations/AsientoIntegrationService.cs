using System.Net.Http.Json;
using Microservicio.ReservasF.Business.Integrations;
using Microservicio.ReservasF.Business.Integrations.Interfaces;

namespace Microservicio.ReservasF.Api.Integrations;

public class AsientoIntegrationService : IAsientoIntegrationService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AsientoIntegrationService(
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    private string? ObtenerToken() =>
        _httpContextAccessor.HttpContext?
            .Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "");

    public async Task<AsientoIntegrationDto?> GetAsientoAsync(
        int idVuelo,
        int idAsiento,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/v1/vuelos/{idVuelo}/asientos/{idAsiento}");

        var token = ObtenerToken();
        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var apiResponse = await response.Content
            .ReadFromJsonAsync<ApiResponseWrapper<AsientoIntegrationDto>>(
                cancellationToken: cancellationToken);

        return apiResponse?.Data;
    }

    public async Task<bool> ExisteAsientoAsync(
        int idVuelo,
        int idAsiento,
        CancellationToken cancellationToken = default)
    {
        var asiento = await GetAsientoAsync(idVuelo, idAsiento, cancellationToken);
        return asiento != null;
    }

    public async Task<bool> AsientoDisponibleAsync(
        int idVuelo,
        int idAsiento,
        CancellationToken cancellationToken = default)
    {
        var asiento = await GetAsientoAsync(idVuelo, idAsiento, cancellationToken);
        return asiento != null
            && asiento.Disponible
            && asiento.Estado == "ACTIVO"
            && !asiento.Eliminado;
    }

    public async Task<bool> AsientoPerteneceAVueloAsync(
        int idAsiento,
        int idVuelo,
        CancellationToken cancellationToken = default)
    {
        var asiento = await GetAsientoAsync(idVuelo, idAsiento, cancellationToken);
        return asiento != null && asiento.IdVuelo == idVuelo;
    }

    public async Task MarcarAsientoNoDisponibleAsync(
        int idVuelo,
        int idAsiento,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default)
    {
        var token = ObtenerToken();

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

    private sealed class ApiResponseWrapper<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
    }
}