using ApiStock.Interfaces;
using ApiStock.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiStock.Services;

public class EstanteriaService : IEstanteriaService
{
    private readonly StockContext _context;

    public EstanteriaService(StockContext context)
    {
        _context = context;
    }
    public async Task<Estanteria> CreateAsync(Estanteria entidad)
    {
        await _context.Estanterias.AddAsync(entidad);
        await _context.SaveChangesAsync();
        return entidad;
    }

    public async Task<IEnumerable<Cajon>> GetCajonesByEstanteriaIdAsync(int estanteriaId)
    {
        return await _context.Cajones
            .Where(c => c.EstanteriaId == estanteriaId)
            .ToListAsync();
    }

    public Task<Estanteria?> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Estanteria[]> GetAllAsync()
    {
        return await _context.Estanterias.ToArrayAsync();
    }

    public Task<Estanteria?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Estanteria> UpdateAsync(int id, Estanteria entidad)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<object>> ObtenerMapaEstanteriasAsync()
    {
        return await _context.Estanterias
            .Include(e => e.Cajones) 
            .Select(e => new {
                e.EstanteriaId,
                e.Nombre,
                Cajones = e.Cajones.Select(c => new {
                    c.CajonId,
                    c.Codigo
                }).ToList()
            })
            .ToListAsync();
    }
}