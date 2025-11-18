using System.ComponentModel.DataAnnotations;
using Guardia.Dominio.Excepciones;

namespace Guardia.Dominio.Entidades.Personal;
public abstract class Persona
{
    private string _cuil;
    private string _nombre;
    private string _apellido = string.Empty;
    private string _email = string.Empty;

    public string Cuil
    {
        get => _cuil;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new DominioException("El cuil no puede estar vacío");
            if (value.Any(char.IsLetter) || value.Any(char.IsSymbol) || value.Any(char.IsSeparator) || value.Any(char.IsPunctuation)) throw new DominioException("El cuil solo puede contener numeros");
            if (value.Length < 10) throw new DominioException("El cuil no puede tener menos de 10 dígitos");
            if (value.Length > 11) throw new DominioException("El cuil no puede tener más de 11 digitos");
            _cuil = value;
        }
    }

    public string Nombre
    {
        get => _nombre;
        set
        {
            if(string.IsNullOrWhiteSpace(value)) throw new DominioException("El nombre de la persona no puede estar vacío");
            if (value.Any(char.IsSymbol) || value.Any(char.IsPunctuation)) throw new DominioException("El nombre no puede contener caracteres especiales");
            _nombre = value;
        }
    }

    public string Apellido
    {
        get => _apellido;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new DominioException("El apellido de la persona no puede estar vacío");
            if (value.Any(char.IsSymbol) || value.Any(char.IsPunctuation)) throw new DominioException("El apellido no puede contener caracteres especiales");
            _apellido = value;
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new DominioException("El email de la persona no puede estar vacío");
            if(!new EmailAddressAttribute().IsValid(value)) throw new DominioException("El email no cumple con el formato requerido");
            _email = value;
        }
    }

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
