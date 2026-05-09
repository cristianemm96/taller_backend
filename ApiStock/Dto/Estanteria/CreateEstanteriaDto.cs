using System.ComponentModel.DataAnnotations;

namespace ApiStock.Dto.Estanteria;
public class CreateEstanteriaDto {
   [Required]
    public string Nombre { get; set; } = string.Empty; 
    
    [Range(0, 100)]
    public int CantidadCajones { get; set; } // C
}