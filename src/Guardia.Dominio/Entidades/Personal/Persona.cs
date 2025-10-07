namespace Guardia.Dominio.Entidades.Personal;
public abstract class Persona
{
    public ulong Dni {  get; set; }
    public ulong Cuil { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Email { get; set; }
    public ulong Telefono { get; set; }
    public string NombreCompleto { get; set; }
    public string Direccion {  get; set; }
    public string Provincia { get; set; }
    public string Pais { get; set; }

}
