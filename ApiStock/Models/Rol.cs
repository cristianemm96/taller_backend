using System.ComponentModel.DataAnnotations;
namespace ApiStock.Models;

public class Rol
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; }
    public virtual ICollection<Usuario> Usuarios { get; set; }
}