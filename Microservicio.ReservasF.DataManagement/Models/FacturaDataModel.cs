namespace Microservicio.ReservasF.DataManagement.Models
{
    public class FacturaDataModel
    {
        public int IdFactura { get; set; }

        public Guid GuidFactura { get; set; }

        public int IdCliente { get; set; }

        public int IdReserva { get; set; }

        public string NumeroFactura { get; set; } = null!;

        public DateTime FechaEmision { get; set; }

        public decimal Subtotal { get; set; }

        public decimal ValorIva { get; set; }

        public decimal CargoServicio { get; set; }

        public decimal Total { get; set; }

        public string? ObservacionesFactura { get; set; }

        public string? OrigenCanalFactura { get; set; }

        public string EstadoFactura { get; set; } = null!;

        // Alias para compatibilidad con services que usan .Estado
        public string Estado
        {
            get => EstadoFactura;
            set => EstadoFactura = value;
        }

        public DateTime? FechaInhabilitacionUtc { get; set; }

        public bool EsEliminado { get; set; }

        public string CreadoPorUsuario { get; set; } = null!;

        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }

        public string ServicioOrigen { get; set; } = null!;

        public string? MotivoInhabilitacion { get; set; }

        public byte[] RowVersion { get; set; } = null!;
    }
}