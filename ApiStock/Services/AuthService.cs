using ApiStock.Dto.Login;
using ApiStock.Interfaces;
using ApiStock.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiStock.Services
{
    public class AuthService : IAuthService
    {
        private readonly StockContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(StockContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
        {

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);


            if (usuario == null || !usuario.Activo)
            {
                return null;
            }


            bool esPasswordValida = BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.PasswordHash);
            if (!esPasswordValida)
            {
                return null;
            }


            var token = GenerarJwtToken(usuario);

            return new AuthResponseDto
            {
                Token = token,
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Rol = usuario.Rol?.Nombre ?? "SinRol",
                Id= usuario.UsuarioId
            };
        }

        private string GenerarJwtToken(Usuario usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            string rolTexto = usuario.Rol?.Nombre?.Trim() ?? "Mecanico";

            var claims = new[]
            {
                new Claim("sub", usuario.UsuarioId.ToString()),
                new Claim("email", usuario.Email),
                new Claim("name", usuario.Nombre),
                new Claim("role", rolTexto)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}