namespace ApiStock.Interfaces;
using ApiStock.Models;

public interface IEstanteriaService : IService<Estanteria>
{
    Task<IEnumerable<Cajon>> GetCajonesByEstanteriaIdAsync(int estanteriaId);
    Task<IEnumerable<object>> ObtenerMapaEstanteriasAsync();
}