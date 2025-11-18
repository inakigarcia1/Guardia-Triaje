using Guardia.Aplicacion.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guardia.Api.Controllers;
[ApiController]
[Route("api/atencion")]
public class AtencionController : ControllerBase
{
    private readonly IAtencionService _atencionService;
    public AtencionController(IAtencionService atencionService)
    {
        _atencionService = atencionService;
    }

    [HttpGet]
    [Authorize(Roles = "Medico,Enfermero")]
    public async Task<IActionResult> ObtenerTodas()
    {
        var atenciones = await _atencionService.ObtenerTodasAsync();
        return Ok(atenciones);
    }

    [HttpPost("reclamar-siguiente")]
    [Authorize(Roles = "Medico")]
    public async Task<IActionResult> ReclamarSiguiente()
    {
        var ingreso = await _atencionService.ReclamarSiguiente();
        if (ingreso == null)
        {
            return NotFound(new { error = "No hay ingresos pendientes" });
        }
        return Ok(ingreso);
    }

    [HttpPost("registrar")]
    [Authorize(Roles = "Medico")]
    public async Task<IActionResult> RegistrarAtencion([FromBody] RegistroAtencionDto dto)
    {
        try
        {
            await _atencionService.RegistrarAtencion(dto);
            return Ok(new { message = "Atención registrada correctamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
