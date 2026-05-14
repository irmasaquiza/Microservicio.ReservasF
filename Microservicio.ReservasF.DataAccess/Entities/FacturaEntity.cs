using System;
using System.Collections.Generic;

namespace Microservicio.ReservasF.DataAccess.Entities
{
    public class FacturaEntity
    {
        public int IdFactura { get; set; }

        public Guid GuidFactura { get; set; }

        // Referencias
        public int IdCliente { get; set; }

        public int IdReserva { get; set; }

        // Datos factura
        public string NumeroFactura { get; set; } = null!;

        public DateTime FechaEmision { get; set; }

        // Valores económicos
        public decimal Subtotal { get; set; }

        public decimal ValorIva { get; set; }

        public decimal CargoServicio { get; set; }

        public decimal Total { get; set; }

        // Información adicional
        public string? ObservacionesFactura { get; set; }

        public string? OrigenCanalFactura { get; set; }

        // Estado
        public string Estado { get; set; } = null!;

        public DateTime? FechaInhabilitacionUtc { get; set; }

        public bool EsEliminado { get; set; }

        // Auditoría
        public string CreadoPorUsuario { get; set; } = null!;

        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }

        // Integración
        public string ServicioOrigen { get; set; } = null!;

        public string? MotivoInhabilitacion { get; set; }

        // Concurrencia
        // Relaciones internas
        public virtual ReservaEntity Reserva { get; set; } = null!;

        public virtual ICollection<BoletoEntity> Boletos { get; set; }
            = new List<BoletoEntity>();
    }
}