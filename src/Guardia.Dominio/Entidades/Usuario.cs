namespace Guardia.Dominio.Entidades;
public class Usuario
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public Rol Rol { get; set; }
}
