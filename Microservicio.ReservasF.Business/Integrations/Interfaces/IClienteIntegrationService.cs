using Microservicio.ReservasF.Business.Integrations;

namespace Microservicio.ReservasF.Business.Integrations.Interfaces;

public interface IClienteIntegrationService
{
    Task<ClienteIntegrationDto?> GetClienteAsync(
        int idCliente,
        CancellationToken cancellationToken = default);

    Task<PasajeroIntegrationDto?> GetPasajeroAsync(
        int idPasajero,
        CancellationToken cancellationToken = default);

    Task<bool> ExisteClienteAsync(
        int idCliente,
        CancellationToken cancellationToken = default);

    Task<bool> ClienteActivoAsync(
        int idCliente,
        CancellationToken cancellationToken = default);

    Task<bool> ExistePasajeroAsync(
        int idPasajero,
        CancellationToken cancellationToken = default);

    Task<bool> PasajeroActivoAsync(
        int idPasajero,
        CancellationToken cancellationToken = default);

    Task<bool> PasajeroPerteneceAClienteAsync(
        int idPasajero,
        int idCliente,
        CancellationToken cancellationToken = default);
}