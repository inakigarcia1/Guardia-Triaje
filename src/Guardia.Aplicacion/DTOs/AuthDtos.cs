namespace Guardia.Aplicacion.DTOs;
public record LoginDto(string Username, string Password);
public record RegisterDto(string Username, string Email, string Password);
public record AuthResponse(
    bool EsExitoso,
    string Token,
    string Username,
    List<string> Errores
);

public record RegisterResponse(bool EsExitoso, string Mensaje, List<string> Errores);
