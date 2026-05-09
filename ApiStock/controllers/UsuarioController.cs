using ApiStock.Dto.Usuario;
using ApiStock.Models;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
namespace ApiStock.Controllers;

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
        return Ok(new { totalElementos = usuarios.Length, elementos = usuarios });
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
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                UrlFoto = dto.UrlFoto,
                RolId = dto.RolId,
                Activo = true
            };

            var usuarioCreado = await _usuarioService.CreateAsync(nuevoUsuario);
            var responseDto = new UsuarioDto
            {
                UsuarioId = usuarioCreado.UsuarioId,
                Nombre = usuarioCreado.Nombre,
                Email = usuarioCreado.Email,
                UrlFoto = usuarioCreado.UrlFoto,
                NombreRol = usuarioCreado.Rol?.Nombre ?? "Sin Rol",
                Activo = usuarioCreado.Activo
            };
            return CreatedAtAction(nameof(GetById), new { id = responseDto.UsuarioId }, responseDto);
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
}