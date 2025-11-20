using Guardia.Dominio.Entidades.Personal;

namespace Guardia.Dominio.Entidades;
public class Atencion
{
    public DateTime CreadoEn { get; set; } = DateTime.Now;

    private string _diagnosticoPresuntivo = string.Empty;
    public required string DiagnosticoPresuntivo
    {
        get => _diagnosticoPresuntivo;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El diagnóstico presuntivo es obligatorio.");

            _diagnosticoPresuntivo = value;
        }
    }

    private string _procedimientoRealizado = string.Empty;
    public required string ProcedimientoRealizado
    {
        get => _procedimientoRealizado;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El procedimiento realizado es obligatorio.");

            _procedimientoRealizado = value;
        }
    }

    private string _observaciones = string.Empty;
    public required string Observaciones
    {
        get => _observaciones;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Las observaciones son obligatorias.");

            _observaciones = value;
        }
    }

    private Ingreso _ingreso;
    public required Ingreso Ingreso
    {
        get => _ingreso;
        set => _ingreso = value ?? throw new ArgumentException("El ingreso es obligatorio.");
    }

    public required Medico Medico { get; set; }
}