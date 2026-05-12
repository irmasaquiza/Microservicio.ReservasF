namespace Microservicio.ReservasF.DataManagement.Models
{
    public class EquipajeFiltroDataModel
    {
        /// <summary>
        /// Restringe resultados a estos boletos permitidos.
        /// </summary>
        public IReadOnlyCollection<int>? IdsBoletoPermitidos { get; set; }

        public int? IdBoleto { get; set; }

        public string? NumeroEtiqueta { get; set; }

        public string? Tipo { get; set; }

        public string? EstadoEquipaje { get; set; }

        public string? Estado { get; set; }

        public decimal? PesoMinimoKg { get; set; }

        public decimal? PesoMaximoKg { get; set; }

        public DateTime? FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }

        public bool IncluirEliminados { get; set; } = false;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}