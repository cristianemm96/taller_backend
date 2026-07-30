using ApiStock.Dto.Login;
using ApiStock.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiStock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var resultado = await _authService.LoginAsync(loginDto);

            if (resultado == null)
            {
                return Unauthorized(new { mensaje = "Credenciales inválidas o usuario inactivo." });
            }

            return Ok(resultado);
        }
    }
}