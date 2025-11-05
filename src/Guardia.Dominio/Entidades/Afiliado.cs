using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guardia.Dominio.Entidades;
public class Afiliado
{
    public ObraSocial ObraSocial {get; set;  }
    public string NumeroAfiliado { get; set; }
    public Afiliado(ObraSocial obraSocial, string numeroAfiliado)
    {
        ObraSocial = obraSocial;
        NumeroAfiliado = numeroAfiliado;
    }
}

public class ObraSocial
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nombre { get; set; }
    public ObraSocial(string nombre)
    {
        Nombre = nombre;
    }
}
