using Guardia.Aplicacion.DTOs;
using Guardia.Aplicacion.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Guardia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngresoController : ControllerBase
{
    private readonly IngresoService _ingresoService;

    public IngresoController(IngresoService ingresoService)
    {
        _ingresoService = ingresoService;
    }
    
    [HttpPost("registrar")]
    public async Task<IActionResult> RegistrarIngreso([FromBody] RegistroIngresoRequest request)
    {
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
                    dni = resultado.Ingreso.Paciente.Dni,
                    nombre = resultado.Ingreso.Paciente.NombreCompleto
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
                    nombre = resultado.Ingreso.Enfermero.NombreCompleto
                }
            }
        });
    }

    [HttpGet("cola-atencion")]
    public async Task<IActionResult> ObtenerColaAtencion()
    {
        var cola = await _ingresoService.ObtenerColaAtencionAsync();
        
        var colaResponse = cola.Select(i => new
        {
            id = i.Id,
            fechaIngreso = i.FechaIngreso,
            paciente = new
            {
                dni = i.Paciente.Dni,
                nombre = i.Paciente.NombreCompleto
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
}
