
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ApiStock.Models;

public class Repuesto
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string NombreComponente { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string CodReferencia { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public virtual Categoria Categoria { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }
    public int CajonId { get; set; }
    [ForeignKey("CajonId")]
    public virtual Cajon Cajon { get; set; }
    public bool Activo { get; set; }
}