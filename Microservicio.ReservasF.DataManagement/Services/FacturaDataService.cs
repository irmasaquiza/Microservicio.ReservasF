using Microservicio.ReservasF.DataAccess.Repositories.Interfaces;
using Microservicio.ReservasF.DataManagement.Common;
using Microservicio.ReservasF.DataManagement.Interfaces;
using Microservicio.ReservasF.DataManagement.Mappers;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.DataManagement.Services;

public class FacturaDataService : IFacturaDataService
{
    private readonly IFacturaRepository _repo;

    private readonly IUnitOfWork _uow;

    public FacturaDataService(
        IFacturaRepository repo,
        IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    // ============================================
    // CONSULTAS
    // ============================================

    public async Task<DataPagedResult<FacturaDataModel>>
        GetPagedAsync(
            FacturaFiltroDataModel filtro)
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
                filtro.NumeroFactura))
        {
            var numero =
                filtro.NumeroFactura
                    .Trim()
                    .ToUpperInvariant();

            query =
                query.Where(x =>
                    x.NumeroFactura.Contains(numero));
        }

        if (filtro.IdCliente.HasValue)
        {
            query =
                query.Where(x =>
                    x.IdCliente ==
                    filtro.IdCliente.Value);
        }

        if (filtro.IdReserva.HasValue)
        {
            query =
                query.Where(x =>
                    x.IdReserva ==
                    filtro.IdReserva.Value);
        }

        query =
            query
                .OrderByDescending(x =>
                    x.FechaEmision)
                .ThenByDescending(x =>
                    x.IdFactura);

        var total =
            query.Count();

        var items =
            query
                .Skip(
                    (filtro.PageNumber - 1)
                    * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(
                    FacturaDataMapper.ToDataModel)
                .ToList();

        return new DataPagedResult<FacturaDataModel>
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

    public async Task<FacturaDataModel?> GetByIdAsync(
        int id)
    {
        var entity =
            await _repo.ObtenerPorIdAsync(id);

        if (entity == null ||
            entity.EsEliminado)
        {
            return null;
        }

        return FacturaDataMapper
            .ToDataModel(entity);
    }

    public async Task<FacturaDataModel?> GetByGuidAsync(
        Guid guidFactura)
    {
        var entity =
            await _repo
                .ObtenerPorGuidAsync(
                    guidFactura);

        return entity == null
            ? null
            : FacturaDataMapper.ToDataModel(entity);
    }

    public async Task<FacturaDataModel?> GetByNumeroAsync(
        string numeroFactura)
    {
        var entity =
            await _repo
                .ObtenerPorNumeroAsync(
                    numeroFactura);

        return entity == null
            ? null
            : FacturaDataMapper.ToDataModel(entity);
    }

    public async Task<IReadOnlyCollection<FacturaDataModel>>
        GetByClienteAsync(
            int idCliente)
    {
        var data =
            await _repo
                .ObtenerPorClienteAsync(
                    idCliente);

        return data
            .Select(
                FacturaDataMapper.ToDataModel)
            .ToList();
    }

    public async Task<IReadOnlyCollection<FacturaDataModel>>
        GetByReservaAsync(
            int idReserva)
    {
        var data =
            await _repo
                .ObtenerPorReservaAsync(
                    idReserva);

        return data
            .Select(
                FacturaDataMapper.ToDataModel)
            .ToList();
    }

    // ============================================
    // VALIDACIONES
    // ============================================

    public async Task<bool> ExistePorIdAsync(
        int idFactura)
    {
        return await _repo
            .ExistePorIdAsync(idFactura);
    }

    public async Task<bool> ExistePorGuidAsync(
        Guid guidFactura)
    {
        return await _repo
            .ExistePorGuidAsync(guidFactura);
    }

    public async Task<bool> ExistePorNumeroAsync(
        string numeroFactura)
    {
        return await _repo
            .ExistePorNumeroAsync(numeroFactura);
    }

    // ============================================
    // COMANDOS
    // ============================================

    public async Task<FacturaDataModel> CreateAsync(
        FacturaDataModel model)
    {
        var entity =
            FacturaDataMapper
                .ToEntity(model);

        entity.EsEliminado = false;

        entity.Estado = "ABI";

        entity.GuidFactura =
            entity.GuidFactura == Guid.Empty
                ? Guid.NewGuid()
                : entity.GuidFactura;

        entity.FechaRegistroUtc =
            DateTime.UtcNow;

        entity.FechaEmision =
            DateTime.UtcNow;

        await _repo.AgregarAsync(entity);

        await _uow.SaveChangesAsync();

        return FacturaDataMapper
            .ToDataModel(entity);
    }

    public async Task<FacturaDataModel?> UpdateAsync(
        FacturaDataModel model)
    {
        var entity =
            await _repo
                .ObtenerPorIdParaEditarAsync(
                    model.IdFactura);

        if (entity == null ||
            entity.EsEliminado)
        {
            return null;
        }

        FacturaDataMapper
            .UpdateEntity(entity, model);

        entity.FechaModificacionUtc =
            DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        return FacturaDataMapper
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

        entity.Estado = "INA";

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