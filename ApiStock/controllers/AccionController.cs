namespace ApiStock.Controllers;
using ApiStock.Dto.Accion;
using ApiStock.Interfaces;
using ApiStock.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AccionController : ControllerBase
{
    IService<Accion> _accionService;

    public AccionController(IService<Accion> accionService)
    {
        _accionService = accionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccionDto>>> GetAll()
    {
        var acciones = await _accionService.GetAllAsync();
        return Ok(new { totalElementos = acciones.Length, elementos = acciones });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccionDto>> GetById(int id)
    {
        var accion = await _accionService.GetByIdAsync(id);
        if (accion == null)
            return NotFound();
        return Ok(accion);
    }

    [HttpPost]
    public async Task<ActionResult<Accion>> Create([FromBody] CreateAccionDto accion)
    {
        try
        {
            var nuevaAccion = new Accion
            {
                NombreAccion = accion.NombreAccion,
                Descripcion = accion.DescripcionAccion
            };
            var accionCreada = await _accionService.CreateAsync(nuevaAccion);
            return CreatedAtAction(nameof(GetById), new { id = nuevaAccion.AccionId }, accionCreada);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno al intentar crear la accion.", error = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, [FromBody] AccionDto accion)
    {
        try
        {
            var existente = await _accionService.GetByIdAsync(id);
            if (existente == null) return NotFound();
            existente.NombreAccion = accion.NombreAccion;
            existente.Descripcion = accion.DescripcionAccion;
            return NoContent();
        }
        catch (Exception ex)
        {

            return StatusCode(500, new {mensaje = "Error interno al intentar actualizar la acción", error = ex.Message});
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var eliminada = await _accionService.DeleteAsync(id);
            if (eliminada == null) return NotFound();
            return Ok(eliminada);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {mensaje = "Error interno al borrar el repuesto", error = ex.Message});
        }
    }
}