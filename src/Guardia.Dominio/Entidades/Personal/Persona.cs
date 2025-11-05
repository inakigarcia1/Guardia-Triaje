namespace Guardia.Dominio.Entidades.Personal;
public abstract class Persona
{
    public string Cuil { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    protected Persona(string cuil, string nombre, string apellido, string email)
    {
        Cuil = cuil;
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
    }
    protected Persona(string cuil, string nombre)
    {
        Cuil = cuil;
        Nombre = nombre;
    }
}
