namespace Microservicio.ReservasF.DataManagement.Models
{
    public class ReservaDataModel
    {
        public int IdReserva { get; set; }

        public Guid GuidReserva { get; set; }

        public string CodigoReserva { get; set; } = null!;

        public int IdCliente { get; set; }

        public int IdVuelo { get; set; }

        public DateTime FechaReservaUtc { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public decimal SubtotalReserva { get; set; }

        public decimal ValorIva { get; set; }

        public decimal TotalReserva { get; set; }

        public string OrigenCanalReserva { get; set; } = null!;

        public string EstadoReserva { get; set; } = null!;

        public DateTime? FechaConfirmacionUtc { get; set; }

        public DateTime? FechaCancelacionUtc { get; set; }

        public string? MotivoCancelacion { get; set; }

        public bool EsEliminado { get; set; }

        public string CreadoPorUsuario { get; set; } = null!;

        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }

        public string ServicioOrigen { get; set; } = null!;

        public string? ContactoEmail { get; set; }

        public string? ContactoTelefono { get; set; }

        public string? Observaciones { get; set; }

        public DateTime? FechaInhabilitacionUtc { get; set; }

        public string? MotivoInhabilitacion { get; set; }

        public byte[] RowVersion { get; set; } = null!;

        public IReadOnlyCollection<ReservaDetalleDataModel> Detalles { get; set; }
            = Array.Empty<ReservaDetalleDataModel>();

        public int CantidadPasajeros =>
            Detalles.Count;
    }
}