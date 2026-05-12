using Microservicio.ReservasF.DataAccess.Repositories.Interfaces;
using Microservicio.ReservasF.DataManagement.Common;
using Microservicio.ReservasF.DataManagement.Interfaces;
using Microservicio.ReservasF.DataManagement.Mappers;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.DataManagement.Services;

public class BoletoDataService : IBoletoDataService
{
    private readonly IBoletoRepository _repo;

    private readonly IUnitOfWork _uow;

    public BoletoDataService(
        IBoletoRepository repo,
        IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    // ============================================
    // CONSULTAS
    // ============================================

    public async Task<DataPagedResult<BoletoDataModel>>
        GetPagedAsync(
            BoletoFiltroDataModel filtro)
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
                filtro.CodigoBoleto))
        {
            var codigo =
                filtro.CodigoBoleto
                    .Trim()
                    .ToUpperInvariant();

            query =
                query.Where(x =>
                    x.CodigoBoleto.Contains(codigo));
        }

        if (filtro.IdReserva.HasValue)
        {
            query =
                query.Where(x =>
                    x.IdReserva ==
                    filtro.IdReserva.Value);
        }

        if (filtro.IdVuelo.HasValue)
        {
            query =
                query.Where(x =>
                    x.IdVuelo ==
                    filtro.IdVuelo.Value);
        }

        if (!string.IsNullOrWhiteSpace(
                filtro.EstadoBoleto))
        {
            var estadoBoleto =
                filtro.EstadoBoleto
                    .Trim()
                    .ToUpperInvariant();

            query =
                query.Where(x =>
                    x.EstadoBoleto ==
                    estadoBoleto);
        }

        query =
            query
                .OrderByDescending(x =>
                    x.FechaEmision)
                .ThenByDescending(x =>
                    x.IdBoleto);

        var total =
            query.Count();

        var items =
            query
                .Skip(
                    (filtro.PageNumber - 1)
                    * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(
                    BoletoDataMapper.ToDataModel)
                .ToList();

        return new DataPagedResult<BoletoDataModel>
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

    public async Task<BoletoDataModel?> GetByIdAsync(
        int id)
    {
        var entity =
            await _repo.ObtenerPorIdAsync(id);

        if (entity == null ||
            entity.EsEliminado)
        {
            return null;
        }

        return BoletoDataMapper
            .ToDataModel(entity);
    }

    public async Task<BoletoDataModel?> GetByCodigoAsync(
        string codigoBoleto)
    {
        var entity =
            await _repo
                .ObtenerPorCodigoAsync(
                    codigoBoleto);

        return entity == null
            ? null
            : BoletoDataMapper.ToDataModel(entity);
    }

    public async Task<IReadOnlyCollection<BoletoDataModel>>
        GetByReservaAsync(
            int idReserva)
    {
        var data =
            await _repo
                .ObtenerPorReservaAsync(
                    idReserva);

        return data
            .Select(
                BoletoDataMapper.ToDataModel)
            .ToList();
    }

    public async Task<IReadOnlyCollection<BoletoDataModel>>
        GetByVueloAsync(
            int idVuelo)
    {
        var data =
            await _repo
                .ObtenerPorVueloAsync(
                    idVuelo);

        return data
            .Select(
                BoletoDataMapper.ToDataModel)
            .ToList();
    }

    public async Task<IReadOnlyCollection<BoletoDataModel>>
        GetByFacturaAsync(
            int idFactura)
    {
        var data =
            await _repo
                .ObtenerPorFacturaAsync(
                    idFactura);

        return data
            .Select(
                BoletoDataMapper.ToDataModel)
            .ToList();
    }

    // ============================================
    // VALIDACIONES
    // ============================================

    public async Task<bool> ExistePorIdAsync(
        int idBoleto)
    {
        return await _repo
            .ExistePorIdAsync(idBoleto);
    }

    public async Task<bool> ExistePorCodigoAsync(
        string codigoBoleto)
    {
        return await _repo
            .ExistePorCodigoAsync(codigoBoleto);
    }

    // ============================================
    // COMANDOS
    // ============================================

    public async Task<BoletoDataModel> CreateAsync(
        BoletoDataModel model)
    {
        var entity =
            BoletoDataMapper
                .ToEntity(model);

        entity.EsEliminado = false;

        entity.Estado = "ACTIVO";

        entity.EstadoBoleto =
            string.IsNullOrWhiteSpace(
                entity.EstadoBoleto)
                ? "ACTIVO"
                : entity.EstadoBoleto
                    .Trim()
                    .ToUpperInvariant();

        entity.FechaEmision =
            DateTime.UtcNow;

        entity.FechaRegistroUtc =
            DateTime.UtcNow;

        await _repo.AgregarAsync(entity);

        await _uow.SaveChangesAsync();

        return BoletoDataMapper
            .ToDataModel(entity);
    }

    public async Task<BoletoDataModel?> UpdateAsync(
        BoletoDataModel model)
    {
        var entity =
            await _repo
                .ObtenerPorIdParaEditarAsync(
                    model.IdBoleto);

        if (entity == null ||
            entity.EsEliminado)
        {
            return null;
        }

        BoletoDataMapper
            .UpdateEntity(entity, model);

        entity.FechaModificacionUtc =
            DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        return BoletoDataMapper
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

        entity.Estado = "INACTIVO";

        entity.ModificadoPorUsuario =
            modificadoPorUsuario.Trim();

        entity.FechaModificacionUtc =
            DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        return true;
    }
}