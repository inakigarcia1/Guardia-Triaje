using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Guardia.Aplicacion.DTOs;
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
        var user = await _userManager.FindByNameAsync(loginDto.Username);

        if (user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return new AuthResponse(
                EsExitoso: false,
                Token: string.Empty,
                Username: user?.UserName ?? string.Empty,
                Errores: ["Usuario o contraseña no válidos."]);
        }

        var token = await GenerateJwtToken(user);
        return new AuthResponse(
            EsExitoso: true,
            Token: token,
            Username: user.UserName ?? string.Empty,
            Errores: []);
    }

    public async Task<RegisterResponse> RegistrarAsync(RegistroUsuarioDto registroUsuarioDto, string rol)
    {
        var user = new IdentityUser { UserName = registroUsuarioDto.Username, Email = registroUsuarioDto.Email };
        var result = await _userManager.CreateAsync(user, registroUsuarioDto.Password);

        var errores = result.Errors.Select(e => e.Description).ToList();

        if (!result.Succeeded)
            return new RegisterResponse(
                EsExitoso: false,
                Mensaje: "No se pudo crear el usuario.",
                Errores: errores
            );

        await _userManager.AddToRoleAsync(user, rol);
        var claim = new Claim("Matricula", registroUsuarioDto.Matricula);
        await _userManager.AddClaimAsync(user, claim);
        
        if (rol == "Enfermero")
        {
            var enfermero = new Enfermero(registroUsuarioDto.Cuil, registroUsuarioDto.Username, registroUsuarioDto.Matricula);
            await _repositorioEnfermero.CrearAsync(enfermero);
        }
        else if (rol == "Medico")
        {
            var medico = new Medico(registroUsuarioDto.Cuil, registroUsuarioDto.Username, registroUsuarioDto.Matricula);
            await _repositorioMedico.CrearAsync(medico);
        }
        
        return new RegisterResponse(
                EsExitoso: true,
                Mensaje: $"Usuario '{user.UserName}' con rol '{rol}' creado con éxito.",
                Errores: errores
        );
    }

    private async Task<string> GenerateJwtToken(IdentityUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
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
