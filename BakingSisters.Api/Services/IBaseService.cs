using BakingSisters.Api.Models;

namespace BakingSisters.Api.Services;

public interface IBaseService<T> where T : BaseModel
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
} 