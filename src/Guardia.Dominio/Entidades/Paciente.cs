using Guardia.Dominio.Entidades.Personal;

namespace Guardia.Dominio.Entidades;

public class Paciente : Persona
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string NumeroDeAfiliado { get; set; } = string.Empty;
    public List<Ingreso> Ingresos { get; set; } = new();
}
