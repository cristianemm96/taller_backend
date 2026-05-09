using System.ComponentModel.DataAnnotations;
namespace ApiStock.Dto.Usuario;
public class UpdateUsuarioDto
{
    [Required]
    public int UsuarioId { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty;
    public string Telefono {get;set;} = string.Empty;
    public string? UrlFoto { get; set; }
    [Required]
    public int RolId { get; set; }

    public bool Activo { get; set; }
}