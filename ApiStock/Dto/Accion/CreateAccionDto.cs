using System.ComponentModel.DataAnnotations;

namespace ApiStock.Dto.Accion;
public class CreateAccionDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100)]
    public string NombreAccion {get;set;} = string.Empty;
    [MaxLength(200)]
    public string DescripcionAccion {get;set;} = string.Empty;

}