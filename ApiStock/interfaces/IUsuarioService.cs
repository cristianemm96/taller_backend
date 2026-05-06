using ApiStock.Models;
namespace ApiStock.Interfaces;
public interface IUsuarioService
{
    Task<Usuario[]> GetAllAsync();
    Task<Usuario> GetByIdAsync(int id);
    Task<Usuario> CreateAsync(Usuario usuario);
    Task<Usuario> UpdateAsync(int id, Usuario usuario);
    Task<bool> DeleteAsync(int id);
}