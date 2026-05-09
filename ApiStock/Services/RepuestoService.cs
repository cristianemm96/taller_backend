namespace ApiStock.Services;
using ApiStock.Models;
using Microsoft.EntityFrameworkCore;

public class RepuestoService : IService<Repuesto>
{
    private readonly StockContext _context;
    public RepuestoService(StockContext context)
    {
        _context = context;
    }
    public async Task<Repuesto> CreateAsync(Repuesto repuesto)
    {
        var nuevoRepuesto = await _context.Repuestos.AddAsync(repuesto);
        await _context.SaveChangesAsync();
        return nuevoRepuesto.Entity;
    }

    public async Task<Repuesto?> DeleteAsync(int id)
    {
        var repuesto = await _context.Repuestos.FindAsync(id) ?? null;
        if (repuesto == null)
            return null;
        _context.Repuestos.Remove(repuesto);
        await _context.SaveChangesAsync();
        return repuesto;
    }

    public async Task<Repuesto[]> GetAllAsync()
    {
        var repuestos = await _context.Repuestos.ToListAsync();
        return [.. repuestos];
    }

    public async Task<Repuesto?> GetByIdAsync(int id)
    {
        var repuesto = await _context.Repuestos.FindAsync(id) ?? null;
        return repuesto;
    }

    public async Task<Repuesto[]> SearchByTermAsync(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return await _context.Repuestos.ToArrayAsync();
        return await _context.Repuestos.Where(r => r.NombreComponente.ToLower().Contains(term.ToLower())
        ||
         r.CodReferencia.ToLower().Contains(term.ToLower())).ToArrayAsync();
    }

    public async Task<Repuesto> UpdateAsync(int id, Repuesto repuesto)
    {
        var existe = await _context.Repuestos.FindAsync(id) ?? throw new InvalidOperationException("Repuesto not found");
        _context.Entry(existe).CurrentValues.SetValues(repuesto);
        await _context.SaveChangesAsync();
        return repuesto;
    }
}