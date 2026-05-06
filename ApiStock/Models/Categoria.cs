using System.ComponentModel.DataAnnotations;
namespace ApiStock.Models;

public class Categoria
{
    [Key]
    public int CategoriaId { get; set; }
    [Required]
    [MaxLength(60)]
    public string Nombre { get; set; }
    public virtual ICollection<Repuesto> Repuestos { get; set; }
}