using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Guardia.Aplicacion.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Guardia.Aplicacion.Servicios;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthService(
        UserManager<IdentityUser> userManager,
        IConfiguration config,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _config = config;
        _roleManager = roleManager;
    }

    public async Task<AuthResponse> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.Username);

        if (user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return new AuthResponse(
                EsExitoso: true,
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

    public async Task<RegisterResponse> RegistrarAsync(RegisterDto registerDto, string rol)
    {
        var user = new IdentityUser { UserName = registerDto.Username, Email = registerDto.Email };
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        var errores = result.Errors.Select(e => e.Description).ToList();

        if (!result.Succeeded)
            return new RegisterResponse(
                EsExitoso: false,
                Mensaje: "No se pudo crear el usuario.",
                Errores: errores
            );

        await _userManager.AddToRoleAsync(user, "Enfermero");
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
