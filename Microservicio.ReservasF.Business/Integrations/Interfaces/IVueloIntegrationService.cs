using Microservicio.ReservasF.Business.Integrations;

namespace Microservicio.ReservasF.Business.Integrations.Interfaces;

public interface IVueloIntegrationService
{
    Task<VueloIntegrationDto?> ObtenerVueloAsync(
        int idVuelo,
        CancellationToken cancellationToken = default);

    Task<AsientoIntegrationDto?> ObtenerAsientoAsync(
        int idAsiento,
        CancellationToken cancellationToken = default);

    Task<bool> ExisteVueloAsync(
        int idVuelo,
        CancellationToken cancellationToken = default);

    Task<bool> VueloDisponibleParaReservaAsync(
        int idVuelo,
        CancellationToken cancellationToken = default);

    Task<bool> VueloPermiteEmisionAsync(
        int idVuelo,
        CancellationToken cancellationToken = default);

    Task<bool> ExisteAsientoAsync(
        int idAsiento,
        CancellationToken cancellationToken = default);

    Task<bool> AsientoPerteneceAVueloAsync(
        int idAsiento,
        int idVuelo,
        CancellationToken cancellationToken = default);

    Task MarcarAsientoNoDisponibleAsync(
        int idAsiento,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default);
}