using Microsoft.EntityFrameworkCore;
namespace ApiStock.Models;

public class StockContext : DbContext
{
    public StockContext(DbContextOptions<StockContext> options) : base(options)
    {
    }   
    public DbSet<Repuesto> Repuestos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Estanteria> Estanterias { get; set; }
    public DbSet<Cajon> Cajones { get; set; }   
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Logs> Logs { get; set; }
    public DbSet<Accion> Acciones { get; set; }
    public DbSet<OrdenTrabajo> OrdenesTrabajo { get; set; }
    public DbSet<DetalleOrden> DetallesOrden { get; set; }
    public DbSet<OrdenTrabajoUsuario> OrdenesTrabajoUsuarios { get; set; }
}    