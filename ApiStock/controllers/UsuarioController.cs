using ApiStock.Interfaces;
using ApiStock.Models;
using Microsoft.AspNetCore.Mvc;
namespace ApiStock.Controllers;

public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
    {
        var usuarios = await _usuarioService.GetAllAsync();
        return Ok(new { totalElementos = usuarios.Length, elementos = usuarios });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetById(int id){
        var usuario = await _usuarioService.GetByIdAsync(id);
        if (usuario == null)
            return NotFound();
        return Ok(usuario);
    }
}