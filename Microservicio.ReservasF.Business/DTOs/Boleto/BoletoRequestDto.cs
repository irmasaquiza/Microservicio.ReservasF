namespace Microservicio.ReservasF.Business.DTOs.Boleto;

public class BoletoRequestDto
{
    public int IdReserva { get; set; }

    public int IdDetalle { get; set; }

    public int IdVuelo { get; set; }

    public int IdAsiento { get; set; }

    public int IdFactura { get; set; }

    public string Clase { get; set; } = null!;

    public decimal PrecioVueloBase { get; set; }

    public decimal PrecioAsientoExtra { get; set; }

    public decimal ImpuestosBoleto { get; set; }

    public decimal CargoEquipaje { get; set; }

    public decimal PrecioFinal { get; set; }
}