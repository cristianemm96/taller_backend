public interface IService<T> where T : class
{
    Task<T[]> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> CreateAsync(T entidad);
    Task<T> UpdateAsync(int id, T entidad);
    Task<T?> DeleteAsync(int id);
}