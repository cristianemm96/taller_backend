using ApiStock.Dto.Orden;
using ApiStock.Models;

namespace ApiStock.Interfaces;

public interface IOrdenTrabajoService
{
    Task<IEnumerable<OrdenTrabajo>> GetAllAsync();
    Task<IEnumerable<OrdenTrabajo>> GetByMecanicoAsync(int mecanicoId);
    Task<OrdenTrabajo> CreateAsync(CrearOrdenTrabajoDto dto);
    Task FinalizarOrdenAsync(int id);
    Task DeleteOrdenAsync(int id);
}