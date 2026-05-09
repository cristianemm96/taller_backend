using System.ComponentModel.DataAnnotations;
namespace ApiStock.Models;

public class Rol
{
    [Key]
    public int RolId { get; set; }
    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;
    [MaxLength(120)]
    public string? Descripcion {get;set;} = string.Empty;
    public virtual ICollection<Usuario> Usuarios { get; set; }
}