using System.ComponentModel.DataAnnotations;

namespace ApiStock.Dto.Categoria;

public class CreateCategoriaDto
{   
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(60)]
    public string NombreCategoria {get; set;} = string.Empty;
}