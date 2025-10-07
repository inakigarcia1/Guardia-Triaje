namespace Guardia.Dominio.Entidades.Personal;

public class Enfermero : Persona
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Matricula { get; set; } = string.Empty;
    public List<Ingreso> IngresosRegistrados { get; set; } = new();
}
