using ApiStock.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiStock.Dto.Orden;
using ApiStock.Interfaces;
namespace ApiStock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdenesController : ControllerBase
{
    private readonly IOrdenTrabajoService _ordenService;

    public OrdenesController(IOrdenTrabajoService context)
    {
        _ordenService = context;
    }

    [HttpGet]
    [Authorize(Roles = "Encargado")]
    public async Task<IActionResult> GetAll()
    {
        var ordenes = await _ordenService.GetAllAsync();
        return Ok(ordenes);
    }

    [HttpGet("mecanico/{mecanicoId:int}")]
    [Authorize(Roles = "Encargado, Mecanico")]
    public async Task<IActionResult> GetByMecanico(int mecanicoId)
    {
        try
        {
            var ordenes = await _ordenService.GetByMecanicoAsync(mecanicoId);
            return Ok(ordenes);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Encargado")]
    public async Task<IActionResult> Create([FromBody] CrearOrdenTrabajoDto dto)
    {
        try
        {
            var nuevaOrden = await _ordenService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = nuevaOrden.Id }, nuevaOrden);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno al crear la orden.", error = ex.Message });
        }
    }

    [HttpPost("{id}/finalizar")]
    [Authorize(Roles = "Encargado, Mecanico")]
    public async Task<IActionResult> FinalizarOrden(int id)
    {
        try
        {
            await _ordenService.FinalizarOrdenAsync(id);
            return Ok(new { message = "Setup completado en el coche de carreras." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al finalizar la orden.", error = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Encargado")]
    public async Task<IActionResult> DeleteOrden(int id)
    {
        try
        {
            await _ordenService.DeleteOrdenAsync(id);
            return Ok(new { message = $"Orden #{id} eliminada con éxito y reserva liberada." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocurrió un error al eliminar la orden.", error = ex.Message });
        }
    }
}