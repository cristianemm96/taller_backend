using ApiStock.Interfaces;
using ApiStock.Models;
using Microsoft.EntityFrameworkCore;
namespace ApiStock.Services;

public class CategoriaService : IService<Categoria>
{
    private readonly StockContext _context;

    public CategoriaService(StockContext context)
    {
        _context = context;
    }
    public async Task<Categoria> CreateAsync(Categoria categoria)
    {
        await _context.Categorias.AddAsync(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }

    public async Task<Categoria?> DeleteAsync(int id)
    {
        var existente = await _context.Categorias.FindAsync(id);
        if (existente == null) return null;
        _context.Categorias.Remove(existente);
        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task<Categoria[]> GetAllAsync()
    {
        var categorias = await _context.Categorias.ToListAsync();
        return [.. categorias];
    }

    public async Task<Categoria?> GetByIdAsync(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id) ?? null;
        return categoria;
    }

    public async Task<Categoria> UpdateAsync(int id, Categoria categoria)
    {
        var existente = await _context.Categorias.FindAsync(id) ?? throw new InvalidOperationException("Categoria no existente");
        _context.Categorias.Entry(existente).CurrentValues.SetValues(categoria);
        await _context.SaveChangesAsync();
        return existente;
    }
}