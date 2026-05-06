using ApiStock.Models;

namespace ApiStock.Interfaces;
public interface IRepuestoService
{
    Task<Repuesto[]> GetAllAsync();
    Task<Repuesto?> GetByIdAsync(int id);
    Task<Repuesto> CreateAsync(Repuesto repuesto);
    Task<Repuesto> UpdateAsync(int id, Repuesto repuesto);
    Task<Repuesto?> DeleteAsync(int id);
    Task<Repuesto[]> SearchByTermAsync(string term);

}