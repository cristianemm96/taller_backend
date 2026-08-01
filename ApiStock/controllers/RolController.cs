using ApiStock.Dto.Rol;
using ApiStock.Interfaces;
using ApiStock.Models;
using Microsoft.AspNetCore.Mvc;
namespace ApiStock.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class RolController : ControllerBase
{

    private readonly IService<Rol> _rolService;
    public RolController(IService<Rol> service)
    {
        _rolService = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RolDto>>> GetAll()
    {
        var roles = await _rolService.GetAllAsync();
        return Ok(new { totalElementos = roles.Length, elementos = roles });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RolDto>> GetRolById(int id)
    {
        var rol = await _rolService.GetByIdAsync(id);
        if (rol == null) return NotFound();
        return Ok(rol);
    }

    [HttpPost]
    public async Task<ActionResult<RolDto>> Create([FromBody] CreatedRolDto dto)
    {
        try
        {
            var nuevoRol = new Rol
            {
                Nombre = dto.Nombre
            };
            var rolCreado = await _rolService.CreateAsync(nuevoRol);
            return StatusCode(201, nuevoRol); 
        }
        catch (Exception ex)
        {
            return StatusCode(400, new {mensaje = "Error al intentar crear el rol", error = ex.Message});
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RolDto rol)
    {
        try
        {
            var existente = await _rolService.GetByIdAsync(id);
            if (existente == null) return NotFound();
            existente.Nombre = rol.Nombre;
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {mensaje = "Error al intentar actualizar el rol", error = ex.Message});
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var eliminada = await _rolService.DeleteAsync(id);
            if (eliminada == null) return NotFound();
            return Ok(eliminada);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {mensaje = "Error al intentar eliminar el rol", error = ex.Message});
        }
    }
}