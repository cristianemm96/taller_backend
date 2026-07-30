using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ApiStock.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPatch("{repuestoId}/mover")]
        public async Task<IActionResult> Mover(int repuestoId, [FromBody] MoverUbicacionDto dto)
        {
            try
            {
                await _stockService.MoverRepuestoAsync(repuestoId, dto.NuevoCajonId, dto.UsuarioId);
                return Ok(new { mensaje = "Repuesto mudado con éxito" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("ajustar")]
        public async Task<IActionResult> AjustarStock([FromBody] AjusteStockDto dto)
        {
            try
            {
                if (dto.Cantidad == 0)
                    return BadRequest(new { message = "La cantidad a ajustar no puede ser cero." });
                await _stockService.AjustarStockDirectoAsync(dto.RepuestoId, dto.Cantidad, dto.UsuarioId);
                return Ok(new { message = "El stock fue actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class MoverUbicacionDto
    {
        public int NuevoCajonId { get; set; }
        public int UsuarioId { get; set; }
    }
}