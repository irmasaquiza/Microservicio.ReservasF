using Microservicio.ReservasF.Business.Integrations;

namespace Microservicio.ReservasF.Business.Integrations.Interfaces;

public interface IAsientoIntegrationService
{
    Task<AsientoIntegrationDto?> GetAsientoAsync(
        int idVuelo,
        int idAsiento,
        CancellationToken cancellationToken = default);

    Task<bool> ExisteAsientoAsync(
        int idVuelo,
        int idAsiento,
        CancellationToken cancellationToken = default);

    Task<bool> AsientoDisponibleAsync(
        int idVuelo,
        int idAsiento,
        CancellationToken cancellationToken = default);

    Task<bool> AsientoPerteneceAVueloAsync(
        int idAsiento,
        int idVuelo,
        CancellationToken cancellationToken = default);

    Task MarcarAsientoNoDisponibleAsync(
        int idVuelo,
        int idAsiento,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default);
}