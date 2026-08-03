using ApiStock.Models;

public class DetalleOrden
{
    public int Id { get; set; }
    
    public int OrdenTrabajoId { get; set; }
    public OrdenTrabajo? OrdenTrabajo { get; set; }

    public int RepuestoId { get; set; }
    public Repuesto? Repuesto { get; set; }

    public int Cantidad { get; set; } 
}