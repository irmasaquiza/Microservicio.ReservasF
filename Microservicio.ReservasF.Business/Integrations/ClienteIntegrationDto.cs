namespace Microservicio.ReservasF.Business.Integrations
{
    public class ClienteIntegrationDto
    {
        public int IdCliente { get; set; }

        public Guid ClienteGuid { get; set; }

        public string TipoIdentificacion { get; set; } = null!;

        public string NumeroIdentificacion { get; set; } = null!;

        public string Nombres { get; set; } = null!;

        public string? Apellidos { get; set; }

        public string? RazonSocial { get; set; }

        public string Correo { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public bool EsEliminado { get; set; }
    }
}