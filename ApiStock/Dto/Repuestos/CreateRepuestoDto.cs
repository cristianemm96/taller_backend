using System.ComponentModel.DataAnnotations;
namespace ApiStock.Dto.Repuestos;
public class CreateRepuestoDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string NombreComponente { get; set; } = string.Empty;

    public string? CodReferencia { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    public int CategoriaId { get; set; }
    public int UbicacionCajon { get; set; }
}