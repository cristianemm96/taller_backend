using ApiStock.Dto.Repuestos;
using ApiStock.Interfaces;
using ApiStock.Models;
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
    public async Task<ActionResult<IEnumerable<RepuestoGetDto>>> GetAll()
    {
        var repuestos = await _repuestoService.GetAllAsync();
        var dto = repuestos.Select(r => new RepuestoGetDto
        {
            Id = r.Id,
            CodReferencia = r.CodReferencia,
            NombreComponente = r.NombreComponente,
            StockDisponible = r.StockDisponible,
            StockFisico = r.StockFisico,
            //CategoriaId = r.CategoriaId,
            NombreCategoria = r.Categoria != null ? r.Categoria.NombreCategoria : "Sin Categoría",
            CajonId = r.CajonId,
            EstanteriaId = r.Cajon != null ? r.Cajon.EstanteriaId : 0,
            CodigoCajon = r.Cajon != null ? r.Cajon.Codigo : "",
            NombreEstanteria = (r.Cajon != null && r.Cajon.Estanteria != null) ? r.Cajon.Estanteria.Nombre : "Sin Estante"
        }).ToArray();

        return Ok(new { totalElementos = repuestos.Length, elementos = dto });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RepuestoDto>> GetById(int id)
    {
        var repuesto = await _repuestoService.GetByIdAsync(id);
        if (repuesto == null)
            return NotFound();
        return Ok(repuesto);
    }

    [HttpPost]
    public async Task<ActionResult<CreateRepuestoDto>> Create(CreateRepuestoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var nuevoRepuesto = new Repuesto
            {
                NombreComponente = dto.NombreComponente,
                CodReferencia = dto.CodReferencia ?? string.Empty,
                StockFisico = dto.StockInicial,
                StockReservado = 0,
                CategoriaId = dto.CategoriaId,
                CajonId = dto.UbicacionCajon,
                Activo = true
            };
            var createdRepuesto = await _repuestoService.CreateAsync(nuevoRepuesto);
            return CreatedAtAction(nameof(GetById), new { id = createdRepuesto.Id }, createdRepuesto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno al crear repuesto", error = ex.Message });
        }

    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRepuestoDto dto)
    {
        try
        {
            var repuestoExistente = await _repuestoService.GetByIdAsync(id);
            if (repuestoExistente == null) return NotFound();
            repuestoExistente.NombreComponente = dto.NombreComponente;
            repuestoExistente.CodReferencia = dto.CodReferencia ?? string.Empty;
            repuestoExistente.CategoriaId = dto.CategoriaId;
            repuestoExistente.CajonId = dto.CajonId;
            repuestoExistente.Activo = dto.Activo;
            await _repuestoService.UpdateAsync(id, repuestoExistente);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno al actualizar los datos del repuesto", error = ex.Message });
        }

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
            return StatusCode(500, new { mensaje = "Error interno al borrar el repuesto", error = ex.Message });
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
    public async Task<ActionResult<IEnumerable<RepuestoDto>>> Search([FromQuery] string term)
    {
        var repuestos = await _repuestoService.SearchByTermAsync(term);
        return Ok(new { totalElementos = repuestos.Length, elementos = repuestos });
    }
}
