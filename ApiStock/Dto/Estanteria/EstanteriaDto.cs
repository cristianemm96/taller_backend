using ApiStock.Dto.Cajon;

namespace ApiStock.Dto.Estanteria;

public class EstanteriaDto
{
    public int EstanteriaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public List<CajonDto> Cajones { get; set; } = new List<CajonDto>();
    public int TotalCajones => Cajones.Count;
}