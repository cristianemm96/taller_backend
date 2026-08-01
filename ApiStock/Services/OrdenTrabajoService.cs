using ApiStock.Dto.Orden;
using ApiStock.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiStock.Services;

public class OrdenTrabajoService
{
    private readonly StockContext _context;

    public OrdenTrabajoService(StockContext context)
    {
        _context = context;
    }

    public async Task<OrdenMecanicoDto> CrearAsync(CrearOrdenTrabajoDto dto)
    {
        var nuevaOrden = new OrdenTrabajo
        {
            DescripcionTrabajo = dto.DescripcionTrabajo,
            FechaCreacion = DateTime.UtcNow,
            Estado = "Abierta"
        };

        if (dto.Detalles != null && dto.Detalles.Any())
        {
            nuevaOrden.Detalles = dto.Detalles.Select(d => new DetalleOrden
            {
                RepuestoId = d.RepuestoId,
                Cantidad = d.Cantidad
            }).ToList();
        }

        _context.OrdenesTrabajo.Add(nuevaOrden);
        await _context.SaveChangesAsync();
        if (dto.MecanicoId > 0)
        {
            var asignacion = new OrdenTrabajoUsuario
            {
                OrdenTrabajoId = nuevaOrden.Id,
                UsuarioId = dto.MecanicoId
            };
            _context.Set<OrdenTrabajoUsuario>().Add(asignacion);
            await _context.SaveChangesAsync();
        }
        return new OrdenMecanicoDto
        {
            Id = nuevaOrden.Id,
            DescripcionTrabajo = nuevaOrden.DescripcionTrabajo,
            FechaCreacion = nuevaOrden.FechaCreacion,
            Estado = nuevaOrden.Estado,
            MecanicoId = dto.MecanicoId,
            CantidadDetalles = nuevaOrden.Detalles.Count
        };
    }

    public async Task<IEnumerable<OrdenMecanicoDto>> ObtenerPorMecanicoAsync(int usuarioId)
    {
        return await _context.OrdenesTrabajo
            .Include(o => o.MecanicoAsignado)
                .ThenInclude(m => m.Usuario)
            .Include(o => o.Detalles)
            .Where(o => o.MecanicoAsignado != null && o.MecanicoAsignado.UsuarioId == usuarioId)
            .Select(o => new OrdenMecanicoDto
            {
                Id = o.Id,
                DescripcionTrabajo = o.DescripcionTrabajo,
                FechaCreacion = o.FechaCreacion,
                Estado = o.Estado,
                MecanicoId = o.MecanicoAsignado.UsuarioId,
                NombreMecanico = o.MecanicoAsignado.Usuario != null ? o.MecanicoAsignado.Usuario.Nombre : "Sin Asignar",
                CantidadDetalles = o.Detalles.Count
            })
            .ToListAsync();
    }

    public async Task<bool> CambiarEstadoAsync(int ordenId, string nuevoEstado)
    {
        var orden = await _context.OrdenesTrabajo.FindAsync(ordenId);
        if (orden == null) return false;

        orden.Estado = nuevoEstado;
        await _context.SaveChangesAsync();
        return true;
    }
}