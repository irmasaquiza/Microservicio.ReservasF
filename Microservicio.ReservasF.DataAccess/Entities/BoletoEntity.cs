using System;
using System.Collections.Generic;

namespace Microservicio.ReservasF.DataAccess.Entities
{
    public class BoletoEntity
    {
        public int IdBoleto { get; set; }

        // Concurrencia
        public byte[] RowVersion { get; set; } = null!;

        // Relaciones internas
        public int IdReserva { get; set; }

        public int IdDetalle { get; set; }

        public int IdFactura { get; set; }

        // Referencias lógicas
        public int IdVuelo { get; set; }

        public int IdAsiento { get; set; }

        // Datos boleto
        public string CodigoBoleto { get; set; } = null!;

        public string Clase { get; set; } = null!;

        // Valores económicos
        public decimal PrecioVueloBase { get; set; }

        public decimal PrecioAsientoExtra { get; set; }

        public decimal ImpuestosBoleto { get; set; }

        public decimal CargoEquipaje { get; set; }

        public decimal PrecioFinal { get; set; }

        // Estado
        public string EstadoBoleto { get; set; } = null!;

        public DateTime FechaEmision { get; set; }

        public bool EsEliminado { get; set; }

        public string Estado { get; set; } = null!;

        // Auditoría
        public string CreadoPorUsuario { get; set; } = null!;

        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }

        // Relaciones internas
        public virtual ReservaEntity Reserva { get; set; } = null!;

        public virtual ReservaDetalleEntity Detalle { get; set; } = null!;

        public virtual FacturaEntity Factura { get; set; } = null!;

        public virtual ICollection<EquipajeEntity> Equipajes { get; set; }
            = new List<EquipajeEntity>();
    }
}