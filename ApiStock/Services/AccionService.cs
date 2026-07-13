using ApiStock.Interfaces;
using ApiStock.Models;
using Microsoft.EntityFrameworkCore;

class AccionService : IService<Accion>
{
    private readonly StockContext _context;

    public AccionService(StockContext context)
    {
        _context = context;
    }

    public async Task<Accion> CreateAsync(Accion entidad)
    {
        var nuevaAccion = await _context.Acciones.AddAsync(entidad);
        await _context.SaveChangesAsync();
        return nuevaAccion.Entity;
    }

    public async Task<Accion?> DeleteAsync(int id)
    {
        Accion? existente = await GetEntidad(id);
        if (existente == null) return null;
        _context.Acciones.Remove(existente);
        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task<Accion[]> GetAllAsync()
    {
        var acciones = await _context.Acciones.ToListAsync();
        return [.. acciones];
    }

    public async Task<Accion?> GetByIdAsync(int id)
    {
        var accion = await _context.Acciones.FindAsync(id) ?? null;
        return accion;
    }

    public async Task<Accion> UpdateAsync(int id, Accion accion)
    {
        Accion? existente = await GetEntidad(id) ?? throw new InvalidOperationException("Accion no existente");
        _context.Acciones.Entry(existente).CurrentValues.SetValues(accion);
        await _context.SaveChangesAsync();
        return existente;
    }

    private async Task<Accion?> GetEntidad(int id)
    {
        return await _context.Acciones.FindAsync(id);
    }
}