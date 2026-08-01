using ApiStock.Dto.Usuario;
using ApiStock.Models;
using Microsoft.AspNetCore.Mvc;
using ApiStock.Interfaces;
using Microsoft.AspNetCore.Authorization;
namespace ApiStock.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsuarioController : ControllerBase
{
    private readonly IService<Usuario> _usuarioService;

    public UsuarioController(IService<Usuario> usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
    {
        var usuarios = await _usuarioService.GetAllAsync();
        var elementosDto = usuarios.Select(u => new UsuarioDto
    {
        Id = u.UsuarioId,
        Nombre = u.Nombre,
        Email = u.Email,
        Telefono = u.Telefono,
        Rol = u.Rol != null ? u.Rol.Nombre.ToLower() : "sin rol", 
         Activo = u.Activo,
    }).ToArray();
        return Ok(new { totalElementos = usuarios.Length, elementos = elementosDto });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioDto>> GetById(int id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);
        if (usuario == null)
            return NotFound();
        return Ok(usuario);
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioDto>> Create([FromBody] CreateUsuarioDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var nuevoUsuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Telefono = dto.Telefono,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RolId = dto.RolId,
                Activo = true
            };

            var usuarioCreado = await _usuarioService.CreateAsync(nuevoUsuario);
            var responseDto = new UsuarioDto
            {
                Id = usuarioCreado.UsuarioId,
                Nombre = usuarioCreado.Nombre,
                Telefono = usuarioCreado.Telefono,
                Email = usuarioCreado.Email,
                Rol = usuarioCreado.Rol?.Nombre ?? "Sin Rol",
                Activo = usuarioCreado.Activo
            };
            return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al crear usuario", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUsuarioDto dto)
    {
        if (id != dto.UsuarioId) return BadRequest("El ID de la URL no coincide con el del cuerpo.");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var usuarioExistente = await _usuarioService.GetByIdAsync(id);
            if (usuarioExistente == null) return NotFound("Usuario no encontrado.");
            usuarioExistente.Nombre = dto.Nombre;
            usuarioExistente.UrlFoto = dto.UrlFoto;
            usuarioExistente.RolId = dto.RolId;
            usuarioExistente.Activo = dto.Activo;
            await _usuarioService.UpdateAsync(id, usuarioExistente);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al actualizar", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var eliminada = await _usuarioService.DeleteAsync(id);
            if(eliminada == null) return NotFound();
            return Ok(eliminada);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {mensaje = "Error al intentar borrar usuario", error = ex.Message});
        }
    }
}