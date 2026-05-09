using System.ComponentModel.DataAnnotations;

namespace ApiStock.Models;

public class Usuario
{
    [Key]
    public int UsuarioId { get; set; }
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string PasswordHash { get; set; } = string.Empty;
    public string? Telefono {get;set;} = string.Empty;
    public string? UrlFoto { get; set; }
    public int RolId { get; set; }
    public virtual Rol Rol { get; set; }
    public bool Activo { get; set; }
}