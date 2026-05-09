using System.ComponentModel.DataAnnotations;
namespace ApiStock.Models;

public class Cajon
{
    [Key]
    public int CajonId { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public bool Ocupado { get; set; }= false;
    public int EstanteriaId { get; set; }
    public virtual ICollection<Repuesto> Repuestos { get; set; } = new List<Repuesto>();
    public virtual Estanteria Estanteria { get; set; }
}