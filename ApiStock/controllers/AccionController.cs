using ApiStock.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
class AccionController : ControllerBase
{
    IAccionService _accionService;

    public AccionController(IAccionService accionService)
    {
        _accionService = accionService;
    }

    [HttpGet]
    public async  Task<ActionResult<IEnumerable<Accion>>> GetAll()
    {
        var acciones = await _accionService.GetAllAsync();
        return Ok(new { totalElementos = acciones.Length, elementos = acciones });
    }
}