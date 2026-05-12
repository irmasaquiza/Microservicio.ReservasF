
using System;
using System.Collections.Generic;

namespace Microservicio.ReservasF.DataAccess.Entities
{
    public class ReservaEntity
    {
        public int IdReserva { get; set; }

        public Guid GuidReserva { get; set; }

        public string CodigoReserva { get; set; } = null!;

        // Referencias lógicas
        public int IdCliente { get; set; }

        public int IdVuelo { get; set; }

        // Fechas
        public DateTime FechaReservaUtc { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        // Valores económicos
        public decimal SubtotalReserva { get; set; }

        public decimal ValorIva { get; set; }

        public decimal TotalReserva { get; set; }

        // Estado
        public string OrigenCanalReserva { get; set; } = null!;

        public string EstadoReserva { get; set; } = null!;

        public DateTime? FechaConfirmacionUtc { get; set; }

        public DateTime? FechaCancelacionUtc { get; set; }

        public string? MotivoCancelacion { get; set; }

        // Auditoría
        public bool EsEliminado { get; set; }

        public string CreadoPorUsuario { get; set; } = null!;

        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }

        // Integración
        public string ServicioOrigen { get; set; } = null!;

        // Contacto
        public string? ContactoEmail { get; set; }

        public string? ContactoTelefono { get; set; }

        public string? Observaciones { get; set; }

        public DateTime? FechaInhabilitacionUtc { get; set; }

        public string? MotivoInhabilitacion { get; set; }

        // Concurrencia
        public byte[] RowVersion { get; set; } = null!;

        // Relaciones INTERNAS
        public virtual ICollection<FacturaEntity> Facturas { get; set; }
            = new List<FacturaEntity>();

        public virtual ICollection<BoletoEntity> Boletos { get; set; }
            = new List<BoletoEntity>();

        public virtual ICollection<ReservaDetalleEntity> Detalles { get; set; }
            = new List<ReservaDetalleEntity>();
    }
}