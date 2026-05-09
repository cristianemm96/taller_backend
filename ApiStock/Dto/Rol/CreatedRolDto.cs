using System.ComponentModel.DataAnnotations;

namespace ApiStock.Dto.Rol;

public class CreatedRolDto
{
    [MaxLength(50)]
    public string Nombre {get;set;} = string.Empty;
    public string Descripcion {get;set;} = string.Empty;
}