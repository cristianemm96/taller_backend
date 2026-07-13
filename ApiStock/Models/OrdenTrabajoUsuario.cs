namespace ApiStock.Models;
public class OrdenTrabajoUsuario
{
    public int Id { get; set; }

    public int OrdenTrabajoId { get; set; }
    public OrdenTrabajo? OrdenTrabajo { get; set; }

    // 🎯 Acá enganchamos con tu tabla de Usuarios existente
    public int UsuarioId { get; set; } 
    public Usuario? Usuario { get; set; } // O como se llame tu entidad de usuarios
}