using ApiStock.Dto.Cajon;
using ApiStock.Interfaces;
using ApiStock.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiStock.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Encargado")]
public class CajonController : ControllerBase
{
    private readonly IService<Cajon> _cajonService;

    public CajonController(IService<Cajon> cajonService)
    {
        _cajonService = cajonService; 
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CajonDto>> GetById(int id)
    {
        var cajon = await _cajonService.GetByIdAsync(id);
        if (cajon == null) return NotFound("El cajón no existe.");
        
        return Ok(cajon);
    }

}