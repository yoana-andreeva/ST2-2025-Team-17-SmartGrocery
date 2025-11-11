using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartGrocery.Repositories
{
    /// <summary>
    /// Generic Repository Pattern - интерфейс, който дефинира общи CRUD операции.
    /// Този патърн отделя логиката за достъп до данни от контролерите.
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
