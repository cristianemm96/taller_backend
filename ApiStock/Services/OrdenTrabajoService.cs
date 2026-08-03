using ApiStock.Dto.Orden;
using ApiStock.Interfaces;
using ApiStock.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiStock.Services;

public class OrdenTrabajoService : IOrdenTrabajoService
{
    private readonly StockContext _context;

    public OrdenTrabajoService(StockContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrdenTrabajo>> GetAllAsync()
    {
        return await _context.OrdenesTrabajo
            .Include(o => o.MecanicoAsignado)
                .ThenInclude(ou => ou.Usuario)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Repuesto)
            .OrderByDescending(o => o.FechaCreacion)
            .ToListAsync();
    }

    public async Task<IEnumerable<OrdenTrabajo>> GetByMecanicoAsync(int mecanicoId)
    {
        var existeMecanico = await _context.Set<Usuario>().AnyAsync(u => u.UsuarioId == mecanicoId);
        if (!existeMecanico)
        {
            throw new KeyNotFoundException($"No se encontró ningún mecánico con el ID {mecanicoId}.");
        }

        return await _context.OrdenesTrabajo
            .Include(o => o.MecanicoAsignado)
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Repuesto)
                    .ThenInclude(r => r.Cajon)
                        .ThenInclude(c => c.Estanteria)
            .Where(o => o.MecanicoAsignado != null && o.MecanicoAsignado.UsuarioId == mecanicoId)
            .OrderByDescending(o => o.FechaCreacion)
            .ToListAsync();
    }

    public async Task<OrdenTrabajo> CreateAsync(CrearOrdenTrabajoDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "Datos de la orden inválidos.");

        var existeUsuario = await _context.Set<Usuario>().AnyAsync(u => u.UsuarioId == dto.MecanicoId);
        if (!existeUsuario)
        {
            throw new KeyNotFoundException($"El mecánico con ID {dto.MecanicoId} no existe en el sistema.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var nuevaOrden = new OrdenTrabajo
            {
                DescripcionTrabajo = dto.DescripcionTrabajo,
                Estado = "Abierta",
                FechaCreacion = DateTime.UtcNow,
                MecanicoAsignado = new OrdenTrabajoUsuario
                {
                    UsuarioId = dto.MecanicoId
                },
                Detalles = dto.Detalles.Select(d => new DetalleOrden
                {
                    RepuestoId = d.RepuestoId,
                    Cantidad = d.Cantidad
                }).ToList()
            };

            _context.OrdenesTrabajo.Add(nuevaOrden);
            await _context.SaveChangesAsync();

            // Validar stock disponible e incrementar reserva
            foreach (var detalle in nuevaOrden.Detalles)
            {
                var repuesto = await _context.Set<Repuesto>().FindAsync(detalle.RepuestoId)
                    ?? throw new KeyNotFoundException($"El componente con ID {detalle.RepuestoId} no existe en el inventario.");

                if (repuesto.StockDisponible < detalle.Cantidad)
                {
                    throw new InvalidOperationException($"Falta de material para {repuesto.NombreComponente}. Disponibles libres: {repuesto.StockDisponible}, Solicitados para setup: {detalle.Cantidad}");
                }

                repuesto.StockReservado += detalle.Cantidad;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return nuevaOrden;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task FinalizarOrdenAsync(int id)
    {
        var orden = await _context.OrdenesTrabajo
            .Include(o => o.Detalles)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (orden == null)
            throw new KeyNotFoundException("La orden no existe.");

        if (orden.Estado.Equals("Finalizada", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Esta orden ya fue cerrada.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var detalle in orden.Detalles)
            {
                var repuesto = await _context.Set<Repuesto>().FindAsync(detalle.RepuestoId);
                if (repuesto != null)
                {
                    // Descuenta del estante físico y retira la reserva
                    repuesto.StockFisico -= detalle.Cantidad;
                    repuesto.StockReservado -= detalle.Cantidad;

                    if (repuesto.StockReservado < 0) repuesto.StockReservado = 0;
                    if (repuesto.StockFisico < 0) repuesto.StockFisico = 0;
                }
            }

            orden.Estado = "Finalizada";

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteOrdenAsync(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var orden = await _context.OrdenesTrabajo
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.Repuesto)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null)
                throw new KeyNotFoundException($"No se encontró la orden #{id}.");

            if (orden.Estado.Equals("Finalizada", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("No se puede eliminar una orden que ya fue finalizada.");

            // Liberar únicamente el stock reservado de la orden abierta
            if (orden.Detalles != null && orden.Detalles.Any())
            {
                foreach (var detalle in orden.Detalles)
                {
                    if (detalle.Repuesto != null)
                    {
                        detalle.Repuesto.StockReservado -= detalle.Cantidad;
                        if (detalle.Repuesto.StockReservado < 0)
                        {
                            detalle.Repuesto.StockReservado = 0;
                        }
                    }
                }
            }

            _context.OrdenesTrabajo.Remove(orden);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}