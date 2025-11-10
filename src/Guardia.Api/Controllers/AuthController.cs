using Guardia.Aplicacion.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Guardia.Aplicacion.Servicios;
using FluentValidation;

namespace Guardia.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegisterDto dto, [FromServices] IValidator<RegisterDto> validator)
    {
        var resultadoValidacion = await validator.ValidateAsync(dto);
        if (!resultadoValidacion.IsValid)
        {
            return BadRequest(resultadoValidacion.Errors.Select(e => e.ErrorMessage));
        }

        var resultado = await _authService.RegistrarAsync(dto, "Enfermero");

        if(resultado.EsExitoso)
            return Created(string.Empty, resultado);

        return BadRequest(resultado);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, [FromServices] IValidator<LoginDto> validator)
    {
        var resultadoValidacion = await validator.ValidateAsync(dto);
        if (!resultadoValidacion.IsValid)
        {
            return BadRequest(resultadoValidacion.Errors.Select(e => e.ErrorMessage));
        }
        var resultado = await _authService.LoginAsync(dto);

        if (resultado.EsExitoso)
            return Ok(resultado);

        return Unauthorized(resultado);
    }

    [HttpGet("quiensoy")]
    [Authorize]
    public IActionResult QuienSoy()
    {
        var username = User.Identity?.Name;
        return Ok(new { Message = $"Estás autenticado como: {username}" });
    }
}
