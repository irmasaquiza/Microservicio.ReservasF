namespace Microservicio.ReservasF.DataManagement.Models
{
    public class BoletoFiltroDataModel
    {
        public string? CodigoBoleto { get; set; }

        public int? IdReserva { get; set; }

        public int? IdVuelo { get; set; }

        public int? IdFactura { get; set; }

        public int? IdAsiento { get; set; }

        public string? Clase { get; set; }

        public string? EstadoBoleto { get; set; }

        public DateTime? FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }

        public bool IncluirEliminados { get; set; } = false;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}