namespace ApiStock.Dto.Repuestos;
public class UpdateRepuestoDto
{
    public int IdRepuesto { get; set; } 
    public string NombreComponente { get; set; } = string.Empty;
    public string CodReferencia { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public int CajonId { get; set; }
    public bool Activo { get; set; }
}