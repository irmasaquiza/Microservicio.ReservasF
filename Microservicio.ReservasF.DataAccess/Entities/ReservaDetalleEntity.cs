
using System;

namespace Microservicio.ReservasF.DataAccess.Entities
{
    public class ReservaDetalleEntity
    {
        public int IdDetalle { get; set; }

        // Concurrencia
        public byte[] RowVersion { get; set; } = null!;

        // FK interna
        public int IdReserva { get; set; }

        // Referencias lógicas
        public int IdPasajero { get; set; }

        public int IdAsiento { get; set; }

        // Valores económicos
        public decimal SubtotalLinea { get; set; }

        public decimal ValorIvaLinea { get; set; }

        public decimal TotalLinea { get; set; }

        // Estado
        public string Estado { get; set; } = null!;

        public bool EsEliminado { get; set; }

        // Auditoría
        public string CreadoPorUsuario { get; set; } = null!;

        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }

        // Relaciones internas
        public virtual ReservaEntity Reserva { get; set; } = null!;

        public virtual BoletoEntity? Boleto { get; set; }
    }
}