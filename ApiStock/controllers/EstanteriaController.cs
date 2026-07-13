using ApiStock.Dto.Cajon;
using ApiStock.Dto.Estanteria;
using ApiStock.Interfaces;
using ApiStock.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiStock.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class EstanteriaController : ControllerBase
{
    private readonly IEstanteriaService _estanteriaService;

    public EstanteriaController(IEstanteriaService service)
    {
        _estanteriaService = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EstanteriaDto>>> GetAll()
    {
        var estanterias = await _estanteriaService.GetAllAsync();
        return Ok(new { total = estanterias.Length, elementos = estanterias });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EstanteriaDto>> GetById(int id)
    {
        var estanteria = await _estanteriaService.GetByIdAsync(id);
        if (estanteria == null) return NotFound();
        return Ok(estanteria);
    }

    [HttpPost]
    public async Task<ActionResult<EstanteriaDto>> Create([FromBody] CreateEstanteriaDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var nuevaEstanteria = new Estanteria { Nombre = dto.Nombre };
            for (int i = 1; i <= dto.CantidadCajones; i++)
            {
                nuevaEstanteria.Cajones.Add(new Cajon
                {
                    Codigo = $"{dto.Nombre[0]}-{i:D2}",
                    Ocupado = false
                });
            }

            var creada = await _estanteriaService.CreateAsync(nuevaEstanteria);

            var responseDto = new EstanteriaDto
            {
                EstanteriaId = creada.EstanteriaId,
                Nombre = creada.Nombre,
                Cajones = creada.Cajones.Select(c => new CajonDto
                {
                    CajonId = c.CajonId,
                    Codigo = c.Codigo,
                    Ocupado = c.Ocupado
                }).ToList()
            };

            return CreatedAtAction(nameof(GetById), new { id = responseDto.EstanteriaId }, responseDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al crear estantería", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var exito = await _estanteriaService.DeleteAsync(id);
            if (exito == null) return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al eliminar estantería", error = ex.Message });
        }
    }

    [HttpGet("{estanteriaId}/cajones")]
    public async Task<ActionResult> GetCajones(int estanteriaId)
    {
        try
        {
            var cajones = await _estanteriaService.GetCajonesByEstanteriaIdAsync(estanteriaId);
            var response = cajones.Select(c => new
            {
                cajonId = c.CajonId,
                codigo = c.Codigo,
                cantidadRepuestos = c.Repuestos?.Count ?? 0
            });
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al obtener los cajones", error = ex.Message });
        }
    }
    [HttpGet("mapa")]
    public async Task<IActionResult> GetMapa()
    {
        try
        {
            var resultado = await _estanteriaService.ObtenerMapaEstanteriasAsync();
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

}