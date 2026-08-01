namespace ApiStock.Models;
public class OrdenTrabajoUsuario
{
    public int Id { get; set; }

    public int OrdenTrabajoId { get; set; }
    public OrdenTrabajo? OrdenTrabajo { get; set; }
    public int UsuarioId { get; set; } 
    public Usuario? Usuario { get; set; } 
}
