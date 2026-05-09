using System.ComponentModel.DataAnnotations;
namespace ApiStock.Models;

public class Categoria
{
    [Key]
    public int CategoriaId { get; set; }
    [Required]
    [MaxLength(60)]
    public string NombreCategoria { get; set; } = string.Empty;
    public virtual ICollection<Repuesto> Repuestos { get; set; }
}