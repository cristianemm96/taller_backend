using ApiStock.Models;

public interface ICategoriaService
{
    Task<Categoria[]> GetAllAsync();
    Task<Categoria?> GetByIdAsync(int id);
    Task<Categoria> CreateAsync(Categoria categoria);
    Task<Categoria> UpdateAsync(int id, Categoria categoria);
    Task<Categoria?> DeleteAsync(int id);
}