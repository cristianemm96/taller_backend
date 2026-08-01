using ApiStock.Dto.Orden;
using ApiStock.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiStock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdenTrabajoController : ControllerBase
{
    private readonly OrdenTrabajoService _ordenTrabajoService;

    public OrdenTrabajoController(OrdenTrabajoService ordenTrabajoService)
    {
        _ordenTrabajoService = ordenTrabajoService;
    }

    [HttpGet("mecanico/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<OrdenMecanicoDto>>> GetPorMecanico(int usuarioId)
    {
        var ordenes = await _ordenTrabajoService.ObtenerPorMecanicoAsync(usuarioId);
        return Ok(ordenes);
    }

    [HttpPut("{id}/estado")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] string nuevoEstado)
    {
        var exito = await _ordenTrabajoService.CambiarEstadoAsync(id, nuevoEstado);
        if (!exito) return NotFound("No se encontró la orden de trabajo.");

        return NoContent();
    }
}