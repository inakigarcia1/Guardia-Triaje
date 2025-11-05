using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guardia.Dominio.Entidades;
public class Domicilio
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Calle { get; set; }
    public int Numero { get; set; }
    public string Localidad { get; set; }
    public string Provincia { get; set; } = "Tucumán";
    public string Pais { get; set; } = "Argentina";
    public Domicilio(string calle, int numero, string localidad)
    {
        Calle = calle;
        Numero = numero;
        Localidad = localidad;
    }
}
