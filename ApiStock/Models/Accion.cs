using System.ComponentModel.DataAnnotations;

namespace ApiStock.Models;

public class Accion
{
    [Key]
    public int AccionId { get; set; }
    [Required]
    [MaxLength(100)]

    public string Nombre { get; set; }
    [MaxLength(200)]
    public string Descripcion { get; set; }
    [Required]
    public virtual ICollection<Logs> Logs { get; set; }
}