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
    public Categoria Categoria { get; set; }
    [Range(0, 6, ErrorMessage = "El stock no puede ser negativo.")]
    public int StockFisico { get; set; } // Lo que hay en el estante
    [Range(0, 6)]
    public int StockReservado { get; set; } // Lo que está prometido a autos en reparación
    [NotMapped]
    public int StockDisponible => StockFisico - StockReservado;
    public int CajonId { get; set; }
    public Cajon Cajon {get;set;}
    public bool Activo { get; set; }
}