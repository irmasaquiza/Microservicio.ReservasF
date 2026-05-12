using Microservicio.ReservasF.DataAccess.Repositories.Interfaces;
using Microservicio.ReservasF.DataManagement.Common;
using Microservicio.ReservasF.DataManagement.Interfaces;
using Microservicio.ReservasF.DataManagement.Mappers;
using Microservicio.ReservasF.DataManagement.Models;

namespace Microservicio.ReservasF.DataManagement.Services;

public class EquipajeDataService : IEquipajeDataService
{
    private readonly IEquipajeRepository _repo;

    private readonly IUnitOfWork _uow;

    public EquipajeDataService(
        IEquipajeRepository repo,
        IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    // ============================================
    // CONSULTAS
    // ============================================

    public async Task<DataPagedResult<EquipajeDataModel>>
        GetPagedAsync(
            EquipajeFiltroDataModel filtro)
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

        if (filtro.IdsBoletoPermitidos
            is { Count: > 0 } permitidos)
        {
            query =
                query.Where(x =>
                    permitidos.Contains(
                        x.IdBoleto));
        }
        else if (filtro.IdsBoletoPermitidos
                 is { Count: 0 })
        {
            query =
                query.Where(_ => false);
        }

        if (filtro.IdBoleto.HasValue)
        {
            query =
                query.Where(x =>
                    x.IdBoleto ==
                    filtro.IdBoleto.Value);
        }

        if (!string.IsNullOrWhiteSpace(
                filtro.NumeroEtiqueta))
        {
            var etiqueta =
                filtro.NumeroEtiqueta
                    .Trim()
                    .ToUpperInvariant();

            query =
                query.Where(x =>
                    x.NumeroEtiqueta
                        .Contains(etiqueta));
        }

        if (!string.IsNullOrWhiteSpace(
                filtro.EstadoEquipaje))
        {
            var estadoEquipaje =
                filtro.EstadoEquipaje
                    .Trim()
                    .ToUpperInvariant();

            query =
                query.Where(x =>
                    x.EstadoEquipaje ==
                    estadoEquipaje);
        }

        if (!string.IsNullOrWhiteSpace(
                filtro.Estado))
        {
            var estado =
                filtro.Estado
                    .Trim()
                    .ToUpperInvariant();

            query =
                query.Where(x =>
                    x.Estado == estado);
        }

        query =
            query.OrderByDescending(x =>
                x.IdEquipaje);

        var total =
            query.Count();

        var items =
            query
                .Skip(
                    (filtro.PageNumber - 1)
                    * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(
                    EquipajeDataMapper.ToDataModel)
                .ToList();

        return new DataPagedResult<EquipajeDataModel>
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

    public async Task<EquipajeDataModel?> GetByIdAsync(
        int id)
    {
        var entity =
            await _repo.ObtenerPorIdAsync(id);

        if (entity == null ||
            entity.EsEliminado)
        {
            return null;
        }

        return EquipajeDataMapper
            .ToDataModel(entity);
    }

    public async Task<EquipajeDataModel?>
        GetByNumeroEtiquetaAsync(
            string numeroEtiqueta)
    {
        var entity =
            await _repo
                .ObtenerPorNumeroEtiquetaAsync(
                    numeroEtiqueta);

        return entity == null
            ? null
            : EquipajeDataMapper.ToDataModel(entity);
    }

    public async Task<IReadOnlyCollection<EquipajeDataModel>>
        GetByBoletoAsync(
            int idBoleto)
    {
        var data =
            await _repo
                .ObtenerPorBoletoAsync(
                    idBoleto);

        return data
            .Select(
                EquipajeDataMapper.ToDataModel)
            .ToList();
    }

    public async Task<IReadOnlyCollection<EquipajeDataModel>>
        GetByTipoAsync(
            string tipo)
    {
        var data =
            await _repo
                .ObtenerPorTipoAsync(
                    tipo);

        return data
            .Select(
                EquipajeDataMapper.ToDataModel)
            .ToList();
    }

    // ============================================
    // VALIDACIONES
    // ============================================

    public async Task<bool> ExistePorIdAsync(
        int idEquipaje)
    {
        return await _repo
            .ExistePorIdAsync(idEquipaje);
    }

    public async Task<bool>
        ExistePorNumeroEtiquetaAsync(
            string numeroEtiqueta)
    {
        return await _repo
            .ExistePorNumeroEtiquetaAsync(
                numeroEtiqueta);
    }

    // ============================================
    // COMANDOS
    // ============================================

    public async Task<EquipajeDataModel> CreateAsync(
        EquipajeDataModel model)
    {
        var entity =
            EquipajeDataMapper
                .ToEntity(model);

        entity.EsEliminado = false;

        entity.Estado = "ACTIVO";

        entity.EstadoEquipaje =
            string.IsNullOrWhiteSpace(
                entity.EstadoEquipaje)
                ? "REGISTRADO"
                : entity.EstadoEquipaje;

        entity.FechaRegistroUtc =
            DateTime.UtcNow;

        await _repo.AgregarAsync(entity);

        await _uow.SaveChangesAsync();

        return EquipajeDataMapper
            .ToDataModel(entity);
    }

    public async Task<EquipajeDataModel?> UpdateAsync(
        EquipajeDataModel model)
    {
        var entity =
            await _repo
                .ObtenerPorIdParaEditarAsync(
                    model.IdEquipaje);

        if (entity == null ||
            entity.EsEliminado)
        {
            return null;
        }

        EquipajeDataMapper
            .UpdateEntity(entity, model);

        entity.FechaModificacionUtc =
            DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        return EquipajeDataMapper
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