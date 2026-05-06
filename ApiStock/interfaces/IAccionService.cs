using ApiStock.Models;

public interface IAccionService
{
    Task<Accion[]> GetAllAsync();
    Task<Accion?> GetByIdAsync(int id);
    Task<Accion> CreateAsync(Accion accion);
    Task<Accion> UpdateAsync(int id, Accion accion);
    Task<Accion?> DeleteAsync(int id);
}