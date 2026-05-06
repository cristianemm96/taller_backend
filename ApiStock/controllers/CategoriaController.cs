using ApiStock.Models;
using Microsoft.AspNetCore.Mvc;

class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriaController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategories()
    {
        var categories = await _categoriaService.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Categoria>> GetCategoryById(int id){
        var category = await _categoriaService.GetByIdAsync(id);
        if (category == null)
            return NotFound();
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<Categoria>> Create(Categoria categoria)
    {
        var createdCategory = await _categoriaService.CreateAsync(categoria);
        return CreatedAtAction(nameof(GetCategories), new { id = createdCategory.CategoriaId }, createdCategory);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
            return BadRequest();

        var updated = await _categoriaService.UpdateAsync(id, categoria);
        if (updated == null)
            return NotFound();

        return NoContent();
    }

    public async Task<IActionResult> Delete(int id){
        try{
            var eliminada = await _categoriaService.DeleteAsync(id);
            if (eliminada == null)
                return NotFound();
            return NoContent();
        }catch{
            return StatusCode(500, "Error interno al borrar la categoría.");
        }
    }
}
