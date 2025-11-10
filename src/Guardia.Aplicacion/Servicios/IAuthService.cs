using Guardia.Aplicacion.DTOs;

namespace Guardia.Aplicacion.Servicios;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginDto loginDto);
    Task<RegisterResponse> RegistrarAsync(RegisterDto registerDto, string rol);
}