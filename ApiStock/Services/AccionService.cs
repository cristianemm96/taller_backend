using ApiStock.Models;

class AccionService : IAccionService
{
    private readonly StockContext _context;

    public AccionService(StockContext context)
    {
        _context = context;
    }
    public async Task<Accion> CreateAsync(Accion accion)
    {
       await _context.AddAsync(accion);
       await _context.
    }

    public Task<Accion?> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Accion[]> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Accion?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Accion> UpdateAsync(int id, Accion accion)
    {
        throw new NotImplementedException();
    }
}