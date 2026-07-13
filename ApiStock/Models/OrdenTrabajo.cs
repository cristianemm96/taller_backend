namespace ApiStock.Models;
public class OrdenTrabajo
{
    public int Id { get; set; }
    
    public string DescripcionTrabajo { get; set; } = string.Empty; 
    
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    
    public string Estado { get; set; } = "Abierta"; 

    public OrdenTrabajoUsuario MecanicoAsignado { get; set; }

    public List<DetalleOrden> Detalles { get; set; } = new();
}