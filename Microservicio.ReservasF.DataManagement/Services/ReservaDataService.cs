using Microservicio.ReservasF.DataAccess.Repositories.Interfaces;
using Microservicio.ReservasF.DataManagement.Common;
using Microservicio.ReservasF.DataManagement.Interfaces;
using Microservicio.ReservasF.DataManagement.Mappers;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.DataManagement.Services;

public class ReservaDataService : IReservaDataService
{
    private readonly IReservaRepository _repo;

    private readonly IUnitOfWork _uow;

    public ReservaDataService(
        IReservaRepository repo,
        IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    // ============================================
    // CONSULTAS
    // ============================================

    public async Task<DataPagedResult<ReservaDataModel>> GetPagedAsync(
        ReservaFiltroDataModel filtro)
    {
        filtro.PageNumber =
            filtro.PageNumber <= 0
                ? 1
                : filtro.PageNumber;

        filtro.PageSize =
            filtro.PageSize <= 0
                ? 10
                : filtro.PageSize;

        var data =
            await _repo.ObtenerTodosAsync();

        var query =
            data.AsQueryable();

        if (!filtro.IncluirEliminados)
        {
            query =
                query.Where(x => !x.EsEliminado);
        }

        if (!string.IsNullOrWhiteSpace(
                filtro.CodigoReserva))
        {
            var codigo =
                filtro.CodigoReserva
                    .Trim()
                    .ToUpperInvariant();

            query =
                query.Where(x =>
                    x.CodigoReserva.Contains(codigo));
        }

        if (filtro.IdCliente.HasValue)
        {
            query =
                query.Where(x =>
                    x.IdCliente ==
                    filtro.IdCliente.Value);
        }

        if (filtro.IdPasajero.HasValue)
        {
            query =
                query.Where(x =>
                    x.Detalles.Any(d =>
                        d.IdPasajero ==
                        filtro.IdPasajero.Value
                        &&
                        !d.EsEliminado));
        }

        if (filtro.IdVuelo.HasValue)
        {
            query =
                query.Where(x =>
                    x.IdVuelo ==
                    filtro.IdVuelo.Value);
        }

        if (!string.IsNullOrWhiteSpace(
                filtro.EstadoReserva))
        {
            var estado =
                filtro.EstadoReserva
                    .Trim()
                    .ToUpperInvariant();

            query =
                query.Where(x =>
                    x.EstadoReserva == estado);
        }

        query =
            query.OrderByDescending(x =>
                x.FechaReservaUtc);

        var total =
            query.Count();

        var items =
            query
                .Skip(
                    (filtro.PageNumber - 1)
                    * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(
                    ReservaDataMapper.ToDataModel)
                .ToList();

        return new DataPagedResult<ReservaDataModel>
        {
            Items = items,

            PaginaActual =
                filtro.PageNumber,

            TamanoPagina =
                filtro.PageSize,

            TotalRegistros =
                total
        };
    }

    public async Task<ReservaDataModel?> GetByIdAsync(
        int id)
    {
        var entity =
            await _repo.ObtenerPorIdAsync(id);

        if (entity == null ||
            entity.EsEliminado)
        {
            return null;
        }

        return ReservaDataMapper
            .ToDataModel(entity);
    }

    public async Task<ReservaDataModel?> GetByGuidAsync(
        Guid guidReserva)
    {
        var entity =
            await _repo.ObtenerPorGuidAsync(
                guidReserva);

        return entity == null
            ? null
            : ReservaDataMapper.ToDataModel(entity);
    }

    public async Task<ReservaDataModel?> GetByCodigoAsync(
        string codigoReserva)
    {
        var entity =
            await _repo.ObtenerPorCodigoAsync(
                codigoReserva);

        return entity == null
            ? null
            : ReservaDataMapper.ToDataModel(entity);
    }

    public async Task<IReadOnlyCollection<ReservaDataModel>>
        GetByClienteAsync(
            int idCliente)
    {
        var data =
            await _repo.ObtenerPorClienteAsync(
                idCliente);

        return data
            .Select(
                ReservaDataMapper.ToDataModel)
            .ToList();
    }

    public async Task<IReadOnlyCollection<ReservaDataModel>>
        GetByPasajeroAsync(
            int idPasajero)
    {
        var data =
            await _repo.ObtenerPorPasajeroAsync(
                idPasajero);

        return data
            .Select(
                ReservaDataMapper.ToDataModel)
            .ToList();
    }

    public async Task<IReadOnlyCollection<ReservaDataModel>>
        GetByVueloAsync(
            int idVuelo)
    {
        var data =
            await _repo.ObtenerPorVueloAsync(
                idVuelo);

        return data
            .Select(
                ReservaDataMapper.ToDataModel)
            .ToList();
    }

    public async Task<ReservaDataModel?>
        GetReservaActivaPorAsientoAsync(
            int idAsiento)
    {
        var entity =
            await _repo
                .ObtenerReservaActivaPorAsientoAsync(
                    idAsiento);

        return entity == null
            ? null
            : ReservaDataMapper.ToDataModel(entity);
    }

    public async Task<ReservaDataModel?>
        GetPorVueloYAsientoAsync(
            int idVuelo,
            int idAsiento)
    {
        var entity =
            await _repo
                .ObtenerPorVueloYAsientoAsync(
                    idVuelo,
                    idAsiento);

        return entity == null
            ? null
            : ReservaDataMapper.ToDataModel(entity);
    }

    public async Task<ReservaDataModel?>
        GetPorVueloYPasajeroAsync(
            int idVuelo,
            int idPasajero)
    {
        var entity =
            await _repo
                .ObtenerPorVueloYPasajeroAsync(
                    idVuelo,
                    idPasajero);

        return entity == null
            ? null
            : ReservaDataMapper.ToDataModel(entity);
    }

    // ============================================
    // VALIDACIONES
    // ============================================

    public async Task<bool> ExistePorIdAsync(
        int idReserva)
    {
        return await _repo
            .ExistePorIdAsync(idReserva);
    }

    public async Task<bool> ExistePorGuidAsync(
        Guid guidReserva)
    {
        return await _repo
            .ExistePorGuidAsync(guidReserva);
    }

    public async Task<bool> ExistePorCodigoAsync(
        string codigoReserva)
    {
        return await _repo
            .ExistePorCodigoAsync(codigoReserva);
    }

    public async Task<bool>
        ExistePorVueloYAsientoAsync(
            int idVuelo,
            int idAsiento)
    {
        return await _repo
            .ExistePorVueloYAsientoAsync(
                idVuelo,
                idAsiento);
    }

    public async Task<bool>
        ExistePorVueloYPasajeroAsync(
            int idVuelo,
            int idPasajero)
    {
        return await _repo
            .ExistePorVueloYPasajeroAsync(
                idVuelo,
                idPasajero);
    }

    // ============================================
    // COMANDOS
    // ============================================

    public async Task<ReservaDataModel> CreateAsync(
        ReservaDataModel model)
    {
        var entity =
            ReservaDataMapper
                .ToEntity(model);

        entity.EsEliminado = false;

        entity.FechaRegistroUtc =
            DateTime.UtcNow;

        entity.FechaReservaUtc =
            DateTime.UtcNow;

        entity.EstadoReserva = "PEN";

        await _repo.AgregarAsync(entity);

        await _uow.SaveChangesAsync();

        return ReservaDataMapper
            .ToDataModel(entity);
    }

    public async Task<ReservaDataModel?> UpdateAsync(
        ReservaDataModel model)
    {
        var entity =
            await _repo
                .ObtenerPorIdParaEditarAsync(
                    model.IdReserva);

        if (entity == null ||
            entity.EsEliminado)
        {
            return null;
        }

        ReservaDataMapper
            .UpdateEntity(entity, model);

        entity.FechaModificacionUtc =
            DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        return ReservaDataMapper
            .ToDataModel(entity);
    }

    public async Task<bool> DeleteAsync(
        int id,
        string modificadoPorUsuario)
    {
        var entity =
            await _repo
                .ObtenerPorIdParaEditarAsync(id);

        if (entity == null ||
            entity.EsEliminado)
        {
            return false;
        }

        entity.EsEliminado = true;

        entity.FechaInhabilitacionUtc =
            DateTime.UtcNow;

        entity.MotivoInhabilitacion =
            "Eliminación lógica";

        entity.ModificadoPorUsuario =
            modificadoPorUsuario.Trim();

        entity.FechaModificacionUtc =
            DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        return true;
    }
}