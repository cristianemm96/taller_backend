namespace ApiStock.Dto.Repuestos;
public class RepuestoGetDto
{
   public int Id { get; set; } 
    public string NombreComponente { get; set; } = string.Empty;
    public string? CodReferencia { get; set; } = string.Empty;
    public int StockDisponible { get; set; }
    public int StockFisico { get; set; } 
    public int CategoriaId { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
    public int? CajonId { get; set; }
    public string CodigoCajon { get; set; } = string.Empty;
    public int EstanteriaId {get;set;}
    public string NombreEstanteria { get; set; } = string.Empty;

}