namespace ApiStock.Dto.Orden;

public class CrearOrdenTrabajoDto
{
    public string DescripcionTrabajo { get; set; } = string.Empty;
    public int MecanicoId { get; set; } 
    public List<DetalleOrdenCreacionDto> Detalles { get; set; } = new();
}