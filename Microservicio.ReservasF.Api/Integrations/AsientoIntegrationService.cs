using Microservicio.ReservasF.Business.Integrations;
using Microservicio.ReservasF.Business.Integrations.Interfaces;

namespace Microservicio.ReservasF.Api.Integrations;

public class AsientoIntegrationService : IAsientoIntegrationService
{
    private readonly HttpClient _httpClient;

    public AsientoIntegrationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AsientoIntegrationDto?> GetAsientoAsync(
        int idAsiento,
        CancellationToken cancellationToken = default)
    {
        // luego aquí llamas al MS Vuelos
        return null;
    }

    public async Task<bool> ExisteAsientoAsync(
        int idAsiento,
        CancellationToken cancellationToken = default)
    {
        var asiento = await GetAsientoAsync(idAsiento, cancellationToken);
        return asiento != null;
    }

    public async Task<bool> AsientoDisponibleAsync(
        int idAsiento,
        CancellationToken cancellationToken = default)
    {
        var asiento = await GetAsientoAsync(idAsiento, cancellationToken);
        return asiento != null && asiento.Disponible && asiento.Estado == "ACTIVO" && !asiento.Eliminado;
    }

    public async Task<bool> AsientoPerteneceAVueloAsync(
        int idAsiento,
        int idVuelo,
        CancellationToken cancellationToken = default)
    {
        var asiento = await GetAsientoAsync(idAsiento, cancellationToken);
        return asiento != null && asiento.IdVuelo == idVuelo;
    }

    public Task MarcarAsientoNoDisponibleAsync(
        int idAsiento,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default)
    {
        // luego aquí llamas al endpoint PATCH del MS Vuelos
        return Task.CompletedTask;
    }
}