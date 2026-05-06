using System.ComponentModel.DataAnnotations;

namespace ApiStock.Models;

public class Usuario
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; }
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }
    [Required]
    [MaxLength(100)]
    public string PasswordHash { get; set; }
    public string? UrlFoto { get; set; }
    public int RolId { get; set; }
    public virtual Rol Rol { get; set; }
    public bool Activo { get; set; }
}