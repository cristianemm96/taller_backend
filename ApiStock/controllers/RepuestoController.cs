using ApiStock.Interfaces;
using ApiStock.Models;
using ApiStock.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiStock.Controllers;


[ApiController]
[Route("api/[controller]")]
public class RepuestoController : ControllerBase
{
    private readonly IRepuestoService _repuestoService;
    private readonly IStockService _stockService;

    public RepuestoController(IRepuestoService repuestoService, IStockService stockService)
    {
        _repuestoService = repuestoService;
        _stockService = stockService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Repuesto>>> GetAll()
    {
        var repuestos = await _repuestoService.GetAllAsync();
        return Ok(new { totalElementos = repuestos.Length, elementos = repuestos });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Repuesto>> GetById(int id)
    {
        var repuesto = await _repuestoService.GetByIdAsync(id);
        if (repuesto == null)
            return NotFound();
        return Ok(repuesto);
    }

    [HttpPost]
    public async Task<ActionResult<Repuesto>> Create(Repuesto repuesto)
    {
        var createdRepuesto = await _repuestoService.CreateAsync(repuesto);
        return CreatedAtAction(nameof(GetById), new { id = createdRepuesto.Id }, createdRepuesto);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, Repuesto repuesto)
    {
        if (id != repuesto.Id)
            return BadRequest();

        var updated = await _repuestoService.UpdateAsync(id, repuesto);
        if (updated == null)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var eliminado = await _repuestoService.DeleteAsync(id);
            if (eliminado == null) return NotFound();
            return Ok(eliminado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error interno al borrar el repuesto: " + ex.Message);
        }
    }

    [HttpPatch("{id}/mover")]
    public async Task<IActionResult> Mover(int id, [FromBody] int nuevoCajonId)
    {
        int usuarioIdSimulado = 1;

        try
        {
            await _stockService.MoverRepuestoAsync(id, nuevoCajonId, usuarioIdSimulado);
            return Ok(new { mensaje = "Repuesto movido con éxito" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Repuesto>>> Search([FromQuery] string term)
    {
        var repuestos = await _repuestoService.SearchByTermAsync(term);
        return Ok(new { totalElementos = repuestos.Length, elementos = repuestos });
    }
}
