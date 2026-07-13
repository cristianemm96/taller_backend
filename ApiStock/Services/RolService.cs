
using ApiStock.Interfaces;
using ApiStock.Models;
using Microsoft.EntityFrameworkCore;

class RolService : IService<Rol>
{
    private readonly StockContext _context;
    public RolService(StockContext context)
    {
        _context = context;
    }
    public async Task<Rol> CreateAsync(Rol entidad)
    {
        var nuevoRol = await _context.Roles.AddAsync(entidad);
        await _context.SaveChangesAsync();
        return nuevoRol.Entity;
    }

    public async Task<Rol?> DeleteAsync(int id)
    {
        var rol = await _context.Roles.FindAsync(id) ?? null;
        if (rol == null) return null;
        _context.Roles.Remove(rol);
        await _context.SaveChangesAsync();
        return rol;
    }

    public async Task<Rol[]> GetAllAsync()
    {
        var roles = await _context.Roles.ToListAsync();
        return [.. roles];
    }

    public async Task<Rol?> GetByIdAsync(int id)
    {
        var rol = await _context.Roles.FindAsync(id) ?? null;
        return rol;
    }

    public async Task<Rol> UpdateAsync(int id, Rol entidad)
    {
        var existe = await _context.Roles.FindAsync(id) ?? throw new InvalidOperationException("Rol not found");
        _context.Entry(existe).CurrentValues.SetValues(entidad);
        await _context.SaveChangesAsync();
        return existe;
    }
}