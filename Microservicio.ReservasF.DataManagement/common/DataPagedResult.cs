namespace Microservicio.ReservasF.DataManagement.Common
{
    public class DataPagedResult<T>
    {
        public IReadOnlyCollection<T> Items { get; set; }
            = Array.Empty<T>();

        public int TotalRegistros { get; set; }

        public int PaginaActual { get; set; }

        public int TamanoPagina { get; set; }

        // Alias para services que usan nombres en inglés
        public int TotalRecords
        {
            get => TotalRegistros;
            set => TotalRegistros = value;
        }

        public int PageNumber
        {
            get => PaginaActual;
            set => PaginaActual = value;
        }

        public int PageSize
        {
            get => TamanoPagina;
            set => TamanoPagina = value;
        }

        public int TotalPaginas =>
            TamanoPagina <= 0
                ? 0
                : (int)Math.Ceiling((double)TotalRegistros / TamanoPagina);

        public int TotalPages => TotalPaginas;

        public bool TienePaginaAnterior =>
            PaginaActual > 1;

        public bool TienePaginaSiguiente =>
            PaginaActual < TotalPaginas;

        public bool HasPreviousPage => TienePaginaAnterior;

        public bool HasNextPage => TienePaginaSiguiente;

        public static DataPagedResult<T> Crear(
            IReadOnlyCollection<T> items,
            int totalRegistros,
            int paginaActual,
            int tamanoPagina)
        {
            return new DataPagedResult<T>
            {
                Items = items,
                TotalRegistros = totalRegistros,
                PaginaActual = paginaActual,
                TamanoPagina = tamanoPagina
            };
        }
    }
}