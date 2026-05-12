namespace Microservicio.ReservasF.Business.DTOs.Equipaje;

public class EquipajeUpdateRequestDto
{
    public string EstadoEquipaje { get; set; } = null!; // REGISTRADO / EMBARCADO / EN_TRANSITO / ENTREGADO / CANCELADO / PERDIDO / DAÑADO
}