using Guardia.Dominio.Excepciones;

namespace Guardia.Dominio.Entidades;
public class Domicilio
{
    private string _localidad;
    private int _numero;
    private string _calle;
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Calle
    {
        get => _calle;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new DominioException("La calle no puede estar vacía");
            if (value.Any(char.IsSymbol) || value.Any(char.IsPunctuation)) throw new DominioException("La calle no puede contener caracteres especiales");

            _calle = value;
        }
    }

    public int Numero
    {
        get => _numero;
        set
        {
            if(value <= 0) throw new DominioException("El número de calle debe ser mayor a cero");
            _numero = value;
        }
    }

    public string Localidad
    {
        get => _localidad;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new DominioException("La localidad no puede estar vacía");
            if (value.Any(char.IsSymbol) || value.Any(char.IsPunctuation)) throw new DominioException("La localidad no puede contener caracteres especiales");
            _localidad = value;
        }
    }

    public string Provincia { get; set; } = "Tucumán";
    public string Pais { get; set; } = "Argentina";
    public Domicilio(string calle, int numero, string localidad)
    {
        Calle = calle;
        Numero = numero;
        Localidad = localidad;
    }
}
