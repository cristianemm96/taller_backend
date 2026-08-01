using ApiStock.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ApiStock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdenesController : ControllerBase
{
    private readonly StockContext _context; 

    public OrdenesController(StockContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Mecanico")]
    public async Task<IActionResult> GetAll()
    {
        var ordenes = await _context.OrdenesTrabajo
            //.Include(o => o.MecanicosAsignados)
                //.ThenInclude(ou => ou.Usuario) // <- Hacer al finalizar usuarios
            .Include(o => o.Detalles)
                .ThenInclude(d => d.Repuesto)
            .OrderByDescending(o => o.FechaCreacion)
            .ToListAsync();

        return Ok(ordenes);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] OrdenTrabajo nuevaOrden)
    {
        if (nuevaOrden == null) return BadRequest("Datos de la orden de competición inválidos.");
        nuevaOrden.Estado = "Abierta";
        nuevaOrden.FechaCreacion = DateTime.UtcNow;

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Set<OrdenTrabajo>().Add(nuevaOrden);
            await _context.SaveChangesAsync();

            foreach (var detalle in nuevaOrden.Detalles)
            {
                var repuesto = await _context.Set<Repuesto>().FindAsync(detalle.RepuestoId) ?? throw new Exception($"El componente con ID {detalle.RepuestoId} no existe en el inventario.");
                if (repuesto.StockDisponible < detalle.Cantidad)
                {
                    throw new Exception($"Falta de material para {repuesto.NombreComponente}. Disponibles libres: {repuesto.StockDisponible}, Solicitados para setup: {detalle.Cantidad}");
                }
                repuesto.StockReservado += detalle.Cantidad;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return CreatedAtAction(nameof(GetAll), new { id = nuevaOrden.Id }, nuevaOrden);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/finalizar")]
    [Authorize(Roles = "Admin,Mecanico")]
    public async Task<IActionResult> FinalizarOrden(int id)
    {
        var orden = await _context.OrdenesTrabajo
            .Include(o => o.Detalles)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (orden == null) return NotFound("La orden no existe.");
        if (orden.Estado == "Finalizada") return BadRequest("Esta orden ya fue cerrada.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var detalle in orden.Detalles)
            {
                var repuesto = await _context.Set<Repuesto>().FindAsync(detalle.RepuestoId);
                if (repuesto != null)
                {
                    repuesto.StockFisico -= detalle.Cantidad;
                    repuesto.StockReservado -= detalle.Cantidad;
                }
            }

            orden.Estado = "Finalizada";

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new { message = "Setup completado en el coche de carreras." });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return BadRequest(new { message = ex.Message });
        }
    }
}