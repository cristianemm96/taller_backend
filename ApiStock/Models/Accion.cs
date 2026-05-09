using System.ComponentModel.DataAnnotations;

namespace ApiStock.Models;

public class Accion
{
    [Key]
    public int AccionId { get; set; }
    [Required]
    [MaxLength(100)]

    public string NombreAccion { get; set; } = string.Empty;
    [MaxLength(200)]
    public string? Descripcion { get; set; } = string.Empty;
    public virtual ICollection<Logs>? Logs { get; set; }
}