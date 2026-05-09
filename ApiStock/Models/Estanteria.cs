using System.ComponentModel.DataAnnotations;
namespace ApiStock.Models;

public class Estanteria
{
    [Key]
    public int EstanteriaId { get; set; }
    public string Nombre {get;set;} = string.Empty;
    public virtual ICollection<Cajon> Cajones { get; set; } = new List<Cajon>();
}