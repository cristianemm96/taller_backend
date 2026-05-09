namespace ApiStock.Dto.Repuestos;
public class RepuestoDto
{
    public int RepuestoId { get; set; }
    public string NombreComponente { get; set; } = string.Empty;
    public string? CodReferencia { get; set; } = string.Empty;
    public string UbicacionCajon { get; set; } = string.Empty; 
    public string CategoriaId = string.Empty;
    public int Stock { get; set; }
}