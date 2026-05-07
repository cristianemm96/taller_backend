using ApiStock.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
class AccionController : ControllerBase
{
    IService<Accion> _accionService;

    public AccionController(IService<Accion> accionService)
    {
        _accionService = accionService;
    }

    [HttpGet]
    public async  Task<ActionResult<IEnumerable<Accion>>> GetAll()
    {
        var acciones = await _accionService.GetAllAsync();
        return Ok(new { totalElementos = acciones.Length, elementos = acciones });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Accion>> GetById(int id)
    {
        var accion = await _accionService.GetByIdAsync(id);
        if (accion == null)
            return NotFound();
        return Ok(accion);
    }

    [HttpPost]
    public async Task<ActionResult<Accion>> Create(Accion accion)
    {
        var nuevaAccion = await _accionService.CreateAsync(accion);
        return CreatedAtAction(nameof(GetById), new { id = nuevaAccion.AccionId }, nuevaAccion);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, Accion accion)
    {
        if (id != accion.AccionId)
            return BadRequest();

        var updated = await _accionService.UpdateAsync(id, accion);
        if (updated == null)
            return NotFound();

        return NoContent();
    }
}