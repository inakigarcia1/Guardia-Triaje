using Guardia.Aplicacion.DTOs;

namespace Guardia.Aplicacion.Servicios;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginDto loginDto);
    Task<RegisterResponse> RegistrarEnfermeroAsync(RegistroEnfermeroDto registroEnfermeroDto);
    Task<RegisterResponse> RegistrarMedicoAsync(RegistroMedicoDto registroEnfermeroDto);
}