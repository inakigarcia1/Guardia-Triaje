namespace Guardia.Aplicacion.DTOs;
public record LoginDto(string Username, string Password);
public record RegistroEnfermeroDto(string Username, string Email, string Password);
public record RegistroMedicoDto(string Username, string Email, string Password, string Matricula);
public record AuthResponse(
    bool EsExitoso,
    string Token,
    string Username,
    List<string> Errores
);

public record RegisterResponse(bool EsExitoso, string Mensaje, List<string> Errores);
