using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guardia.Dominio.Excepciones;

namespace Guardia.Dominio.Entidades;
public class Afiliado
{
    private string _numeroAfiliado;
    public ObraSocial ObraSocial {get; set;  }

    public string NumeroAfiliado
    {
        get => _numeroAfiliado;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new DominioException("El número de afiliado no puede estar vacío");
            _numeroAfiliado = value;
        }
    }

    public Afiliado(ObraSocial obraSocial, string numeroAfiliado)
    {
        ObraSocial = obraSocial;
        NumeroAfiliado = numeroAfiliado;
    }
}