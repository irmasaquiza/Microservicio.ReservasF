namespace Microservicio.ReservasF.DataManagement.Models
{
    public class EquipajeDataModel
    {
        public int IdEquipaje { get; set; }

        public byte[] RowVersion { get; set; } = null!;

        public int IdBoleto { get; set; }

        public string Tipo { get; set; } = null!;

        public decimal PesoKg { get; set; }

        public string? DescripcionEquipaje { get; set; }

        public decimal PrecioExtra { get; set; }

        public string? DimensionesCm { get; set; }

        public string NumeroEtiqueta { get; set; } = null!;

        public string EstadoEquipaje { get; set; } = null!;

        public bool EsEliminado { get; set; }

        public string Estado { get; set; } = null!;

        public string CreadoPorUsuario { get; set; } = null!;

        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }
    }
}