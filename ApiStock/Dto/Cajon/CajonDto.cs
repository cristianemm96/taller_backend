namespace ApiStock.Dto.Cajon;

public class CajonDto
{
    public int CajonId { get; set; }
    public string Codigo { get; set; }
    public bool Ocupado { get; set; }
    public int CantidadDeRepuestos { get; set; }
}