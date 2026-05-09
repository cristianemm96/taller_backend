using System.ComponentModel.DataAnnotations;
namespace ApiStock.Dto.Usuario;
public class CreateUsuarioDto
{
    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string Telefono {get;set;} = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty; 

    public string? UrlFoto { get; set; }

    [Required]
    public int RolId { get; set; }
}