using FluentValidation;
using Guardia.Aplicacion.DTOs;
using Guardia.Aplicacion.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guardia.Api.Controllers;

[ApiController]
[Route("api/ingreso")]
public class IngresoController : ControllerBase
{
    private readonly IIngresoService _ingresoService;

    public IngresoController(IIngresoService ingresoService)
    {
        _ingresoService = ingresoService;
    }
    
    [HttpPost("registrar")]
    [Authorize(Roles = "Enfermero")]
    public async Task<IActionResult> RegistrarIngreso([FromBody] RegistroIngresoRequest request, [FromServices] IValidator<RegistroIngresoRequest> validator)
    {
        var resultadoValidacion = await validator.ValidateAsync(request);
        if (!resultadoValidacion.IsValid)
        {
            return BadRequest(resultadoValidacion.Errors.Select(e => e.ErrorMessage));
        }
        var matricula = User.Claims.FirstOrDefault(c => c.Type == "Matricula")?.Value;
        if(matricula is null)
        {
            return BadRequest(new { error = "Se debe enviar la matricula del enfermero" });
        }
        request.MatriculaEnfermero = matricula;
        var resultado = await _ingresoService.RegistrarIngresoAsync(request);
        if (!resultado.EsExitoso)
        {
            return BadRequest(new { error = resultado.MensajeError });
        }

        return Ok(new 
        { 
            message = resultado.Mensaje,
            ingreso = new
            {
                id = resultado.Ingreso!.Id,
                fechaIngreso = resultado.Ingreso.FechaIngreso,
                paciente = new
                {
                    cuil = resultado.Ingreso.Paciente.Cuil,
                    nombre = resultado.Ingreso.Paciente.Nombre
                },
                nivelEmergencia = new
                {
                    prioridad = resultado.Ingreso.NivelEmergencia.Prioridad.ToString(),
                    color = resultado.Ingreso.NivelEmergencia.Color,
                    tiempoMaximo = resultado.Ingreso.NivelEmergencia.TiempoMaximoMinutos
                },
                estado = resultado.Ingreso.Estado.ToString(),
                enfermero = new
                {
                    matricula = resultado.Ingreso.Enfermero.Matricula,
                    nombre = resultado.Ingreso.Enfermero.Nombre
                }
            }
        });
    }

    [HttpGet("cola-atencion")]
    [Authorize(Roles = "Enfermero, Medico")]
    public async Task<IActionResult> ObtenerColaAtencion()
    {
        var cola = await _ingresoService.ObtenerColaAtencionAsync();
        
        var colaResponse = cola.Select(i => new
        {
            id = i.Id,
            fechaIngreso = i.FechaIngreso,
            paciente = new
            {
                cuil = i.Paciente.Cuil,
                nombre = i.Paciente.Nombre
            },
            nivelEmergencia = new
            {
                prioridad = i.NivelEmergencia.Prioridad.ToString(),
                color = i.NivelEmergencia.Color,
                tiempoMaximo = i.NivelEmergencia.TiempoMaximoMinutos
            },
            estado = i.Estado.ToString(),
            informe = i.Informe,
            frecuenciaCardiaca = i.FrecuenciaCardiaca,
            frecuenciaRespiratoria = i.FrecuenciaRespiratoria,
            tensionArterial = i.TensionArterial.ToString(),
            temperatura = i.Temperatura
        }).ToList();

        return Ok(colaResponse);
    }

    [HttpGet("buscar-por-cuil")]
    [Authorize(Roles = "Enfermero")]
    public async Task<IActionResult> BuscarPacientePorCuil([FromQuery] string cuil)
    {
        if (string.IsNullOrWhiteSpace(cuil))
        {
            return BadRequest(new { error = "El CUIL es obligatorio" });
        }

        var paciente = await _ingresoService.BuscarPacientePorCuilAsync(cuil);
        if (paciente is null)
        {
            return NotFound();
        }

        return Ok(paciente);
    }
}
