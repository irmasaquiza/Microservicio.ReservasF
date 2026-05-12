namespace Microservicio.ReservasF.Business.Integrations
{
    public class VueloIntegrationDto
    {
        public int IdVuelo { get; set; }

        public int IdAeropuertoOrigen { get; set; }

        public int IdAeropuertoDestino { get; set; }

        public string NumeroVuelo { get; set; } = null!;

        public DateTime FechaHoraSalida { get; set; }

        public DateTime FechaHoraLlegada { get; set; }

        public int DuracionMin { get; set; }

        public decimal PrecioBase { get; set; }

        public int CapacidadTotal { get; set; }

        public string EstadoVuelo { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public bool Eliminado { get; set; }
    }
}