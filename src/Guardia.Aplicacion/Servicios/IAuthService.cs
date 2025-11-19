namespace Guardia.Aplicacion.Servicios;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginDto loginDto);
    Task<RegisterResponse> RegistrarAsync(RegistroUsuarioDto registroUsuarioDto, string rol);
}
public record LoginDto(string Username, string Password);
public record RegistroUsuarioDto(string Username, string Email, string Password, string Matricula, string Cuil);
public record AuthResponse(
    bool EsExitoso,
    string Token,
    string Username,
    List<string> Errores,
    string? Rol = null
);

public record RegisterResponse(bool EsExitoso, string Mensaje, List<string> Errores);