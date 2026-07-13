using ApiStock.Models;

namespace ApiStock.Interfaces;
public interface IRepuestoService : IService<Repuesto>
{
    Task<Repuesto[]> SearchByTermAsync(string term);

}