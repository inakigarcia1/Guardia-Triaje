namespace Guardia.Dominio.Entidades.Triajes;
public class Nivel
{
    public PrioridadTriaje Prioridad { get; set; }
    public required string Color { get; set; }
    public int TiempoMaximo { get; set; }
}
