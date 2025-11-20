namespace Guardia.Aplicacion.DTOs;

public class PacienteDto
{
    public string Cuil { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DomicilioDto? Domicilio { get; set; }
    public AfiliadoDto? Afiliado { get; set; }
}

public class DomicilioDto
{
    public string Calle { get; set; } = string.Empty;
    public int Numero { get; set; }
    public string Localidad { get; set; } = string.Empty;
    public string Provincia { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
}

public class AfiliadoDto
{
    public string NombreObraSocial { get; set; } = string.Empty;
    public string NumeroAfiliado { get; set; } = string.Empty;
}

