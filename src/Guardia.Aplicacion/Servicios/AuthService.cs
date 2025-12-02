using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Guardia.Dominio.Entidades.Personal;
using Guardia.Dominio.Repositorios;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Guardia.Aplicacion.Servicios;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IRepositorioEnfermero _repositorioEnfermero;
    private readonly IRepositorioMedico _repositorioMedico;

    public AuthService(
        UserManager<IdentityUser> userManager,
        IConfiguration config,
        RoleManager<IdentityRole> roleManager,
        IRepositorioEnfermero repositorioEnfermero,
        IRepositorioMedico repositorioMedico)
    {
        _userManager = userManager;
        _config = config;
        _roleManager = roleManager;
        _repositorioEnfermero = repositorioEnfermero;
        _repositorioMedico = repositorioMedico;
    }

    public async Task<AuthResponse> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return new AuthResponse(
                EsExitoso: false,
                Token: string.Empty,
                Email: loginDto.Email,
                Errores: ["Usuario o contraseña no válidos."]);
        }
        var token = await GenerateJwtTokenAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        return new AuthResponse(
            EsExitoso: true,
            Token: token,
            Email: user.Email ?? string.Empty,
            Errores: [],
            Rol: roles.FirstOrDefault()
            );
    }

    public async Task<RegisterResponse> RegistrarAsync(RegistroUsuarioDto registroUsuarioDto, string rol)
    {
        var user = new IdentityUser { UserName = registroUsuarioDto.Email, Email = registroUsuarioDto.Email };
        var result = await _userManager.CreateAsync(user, registroUsuarioDto.Password);

        var errores = result.Errors.Select(e => e.Description).ToList();

        if (!result.Succeeded)
            return new RegisterResponse(
                EsExitoso: false,
                Mensaje: "No se pudo crear el usuario.",
                Errores: errores
            );

        await _userManager.AddToRoleAsync(user, rol);

        var claimMatricula = new Claim("Matricula", registroUsuarioDto.Matricula);
        await _userManager.AddClaimAsync(user, claimMatricula);

        switch (rol)
        {
            case "Enfermero":
                {
                    var enfermero = new Enfermero(registroUsuarioDto.Cuil, registroUsuarioDto.Nombre, registroUsuarioDto.Apellido, registroUsuarioDto.Email, registroUsuarioDto.Matricula);
                    await _repositorioEnfermero.CrearAsync(enfermero);
                    break;
                }
            case "Medico":
                {
                    var medico = new Medico(registroUsuarioDto.Cuil, registroUsuarioDto.Nombre, registroUsuarioDto.Apellido, registroUsuarioDto.Email, registroUsuarioDto.Matricula);
                    await _repositorioMedico.CrearAsync(medico);
                    break;
                }
        }
        
        return new RegisterResponse(
                EsExitoso: true,
                Mensaje: $"Usuario '{registroUsuarioDto.Email}' con rol '{rol}' creado con éxito.",
                Errores: errores
        );
    }

    private async Task<string> GenerateJwtTokenAsync(IdentityUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(1);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
