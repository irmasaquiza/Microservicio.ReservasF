namespace Microservicio.ReservasF.DataManagement.Models
{
    public class BoletoDataModel
    {
        public int IdBoleto { get; set; }

        public byte[] RowVersion { get; set; } = null!;

        public int IdReserva { get; set; }

        public int IdDetalle { get; set; }

        public int IdVuelo { get; set; }

        public int IdAsiento { get; set; }

        public int IdFactura { get; set; }

        public string CodigoBoleto { get; set; } = null!;

        public string Clase { get; set; } = null!;

        public decimal PrecioVueloBase { get; set; }

        public decimal PrecioAsientoExtra { get; set; }

        public decimal ImpuestosBoleto { get; set; }

        public decimal CargoEquipaje { get; set; }

        public decimal PrecioFinal { get; set; }

        public string EstadoBoleto { get; set; } = null!;

        public DateTime FechaEmision { get; set; }

        public bool EsEliminado { get; set; }

        public string Estado { get; set; } = null!;

        public string CreadoPorUsuario { get; set; } = null!;

        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }
    }
}