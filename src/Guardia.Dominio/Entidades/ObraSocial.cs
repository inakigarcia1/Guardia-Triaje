using Guardia.Dominio.Excepciones;

namespace Guardia.Dominio.Entidades;

public class ObraSocial
{
    private string _nombre;
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Nombre
    {
        get => _nombre;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new DominioException("El nombre de la obra social no puede estar vacío");
            if (value.Any(char.IsSymbol) || value.Any(char.IsPunctuation)) throw new DominioException("El nombre de la obra social no puede contener caracteres especiales");
            _nombre = value;
        }
    }

    public ObraSocial(string nombre)
    {
        Nombre = nombre;
    }
}