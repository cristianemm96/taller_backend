using ApiStock.Dto.Categoria;
using ApiStock.Interfaces;
using ApiStock.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ApiStock.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Encargado")]
public class CategoriaController : ControllerBase
{
    private readonly IService<Categoria> _categoriaService;

    public CategoriaController(IService<Categoria> categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetAll()
    {
        var categorias = await _categoriaService.GetAllAsync();
        return Ok(new  {totalElementos = categorias.Length, elementos = categorias});
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaDto>> GetCategoriaById(int id)
    {
        var categoria = await _categoriaService.GetByIdAsync(id);
        if (categoria == null)
            return NotFound();
        return Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> Create([FromBody] CreateCategoriaDto categoria)
    {
        try
        {
            var nuevaCategoria = new Categoria
            {
                NombreCategoria = categoria.NombreCategoria,
            };
            var createdCategory = await _categoriaService.CreateAsync(nuevaCategoria);
            return CreatedAtAction(nameof(GetAll), new { id = createdCategory.CategoriaId }, createdCategory);
        }
        catch (Exception  ex)
        {

            return StatusCode(500, new {mensaje = "Error al intentar crear la categoria\n" + ex.Message});
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoriaDto categoria)
    {
        try
        {
            var categoriaExistente = await _categoriaService.GetByIdAsync(id);
            if (categoriaExistente == null) return NotFound();
            categoriaExistente.NombreCategoria = categoria.NombreCategoria;
            await _categoriaService.UpdateAsync(id, categoriaExistente);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {mensaje = "Error al intentar actualizar categoria\n" + ex.Message});
        }

    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var eliminada = await _categoriaService.DeleteAsync(id);
            if (eliminada == null)
                return NotFound();
            return NoContent();
        }
        catch
        {
            return StatusCode(500, new {mensaje = "Error interno al borrar la categoría."});
        }
    }
}
