namespace Microservicio.ReservasF.DataManagement.Models
{
    public class ReservaFiltroDataModel
    {
        public Guid? GuidReserva { get; set; }

        public string? CodigoReserva { get; set; }

        public int? IdCliente { get; set; }

        public int? IdPasajero { get; set; }

        public int? IdVuelo { get; set; }

        public int? IdAsiento { get; set; }

        public string? EstadoReserva { get; set; }

        public string? OrigenCanalReserva { get; set; }

        public DateTime? FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }

        public bool SoloActivas { get; set; }

        public bool IncluirEliminados { get; set; } = false;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}