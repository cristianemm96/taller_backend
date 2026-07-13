using System.ComponentModel.DataAnnotations;

namespace ApiStock.Dto.Estanteria;
public class CreateEstanteriaDto {
   [Required]
    public string Nombre { get; set; } = string.Empty; 
    
    [Range(0, 5)]
    public int CantidadCajones { get; set; }
}