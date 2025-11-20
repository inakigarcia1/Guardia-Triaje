namespace Guardia.Aplicacion.Servicios;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginDto loginDto);
    Task<RegisterResponse> RegistrarAsync(RegistroUsuarioDto registroUsuarioDto, string rol);
}
public record LoginDto(string Email, string Password);
public record RegistroUsuarioDto(string Nombre, string Apellido, string Email, string Password, string Matricula, string Cuil);
public record AuthResponse(
    bool EsExitoso,
    string Token,
    string Email,
    List<string> Errores,
    string? Rol = null
);

public record RegisterResponse(bool EsExitoso, string Mensaje, List<string> Errores);