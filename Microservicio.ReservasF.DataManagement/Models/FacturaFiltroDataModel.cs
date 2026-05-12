namespace Microservicio.ReservasF.DataManagement.Models
{
    public class FacturaFiltroDataModel
    {
        public Guid? GuidFactura { get; set; }

        public string? NumeroFactura { get; set; }

        public int? IdCliente { get; set; }

        public int? IdReserva { get; set; }

        public string? EstadoFactura { get; set; }

        public string? OrigenCanalFactura { get; set; }

        public decimal? TotalMinimo { get; set; }

        public decimal? TotalMaximo { get; set; }

        public DateTime? FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }

        public bool IncluirEliminados { get; set; } = false;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}