using System;

namespace Microservicio.ReservasF.DataAccess.Entities
{
    public class EquipajeEntity
    {
        public int IdEquipaje { get; set; }

        // Concurrencia
        public byte[] RowVersion { get; set; } = null!;

        // FK interna
        public int IdBoleto { get; set; }

        // Datos equipaje
        public string Tipo { get; set; } = null!;

        public decimal PesoKg { get; set; }

        public string? DescripcionEquipaje { get; set; }

        public decimal PrecioExtra { get; set; }

        public string? DimensionesCm { get; set; }

        public string NumeroEtiqueta { get; set; } = null!;

        public string EstadoEquipaje { get; set; } = null!;

        // Estado
        public bool EsEliminado { get; set; }

        public string Estado { get; set; } = null!;

        // Auditoría
        public string CreadoPorUsuario { get; set; } = null!;

        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }

        // Relación interna
        public virtual BoletoEntity Boleto { get; set; } = null!;
    }
}