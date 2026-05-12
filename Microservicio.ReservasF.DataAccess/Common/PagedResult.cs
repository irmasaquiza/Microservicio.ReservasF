using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservicio.ReservasF.DataAccess.Common
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; }

        public int TotalRegistros { get; }

        public int PaginaActual { get; }

        public int TamanoPagina { get; }

        public int TotalPaginas =>
            TamanoPagina <= 0
                ? 0
                : (int)Math.Ceiling(
                    (double)TotalRegistros / TamanoPagina);

        public int TotalItemsCurrentPage =>
            Items.Count;

        public bool TienePaginaAnterior =>
            PaginaActual > 1;

        public bool TienePaginaSiguiente =>
            PaginaActual < TotalPaginas;

        public bool HasItems =>
            Items.Count > 0;

        public PagedResult(
            IEnumerable<T> items,
            int totalRegistros,
            int paginaActual,
            int tamanoPagina)
        {
            if (paginaActual <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(paginaActual),
                    "La página actual debe ser mayor a 0.");
            }

            if (tamanoPagina <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(tamanoPagina),
                    "El tamaño de página debe ser mayor a 0.");
            }

            Items = (items ?? Enumerable.Empty<T>())
                .ToList()
                .AsReadOnly();

            TotalRegistros = totalRegistros;

            PaginaActual = paginaActual;

            TamanoPagina = tamanoPagina;
        }

        public static PagedResult<T> Crear(
            IEnumerable<T> items,
            int totalRegistros,
            int paginaActual,
            int tamanoPagina)
        {
            return new PagedResult<T>(
                items,
                totalRegistros,
                paginaActual,
                tamanoPagina);
        }
    }
}