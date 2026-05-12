namespace Microservicio.ReservasF.Business.Integrations
{
    public class PasajeroIntegrationDto
    {
        public int IdPasajero { get; set; }

        public int? IdCliente { get; set; }

        public string NombrePasajero { get; set; } = null!;

        public string ApellidoPasajero { get; set; } = null!;

        public string TipoDocumentoPasajero { get; set; } = null!;

        public string NumeroDocumentoPasajero { get; set; } = null!;

        public DateTime? FechaNacimientoPasajero { get; set; }

        public string? NacionalidadPasajero { get; set; }

        public string? EmailContactoPasajero { get; set; }

        public string? TelefonoContactoPasajero { get; set; }

        public string? GeneroPasajero { get; set; }

        public bool RequiereAsistencia { get; set; }

        public string? ObservacionesPasajero { get; set; }

        public string Estado { get; set; } = null!;

        public bool EsEliminado { get; set; }
    }
}