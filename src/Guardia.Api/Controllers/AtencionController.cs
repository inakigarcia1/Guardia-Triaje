using Guardia.Aplicacion.Servicios;
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
}
