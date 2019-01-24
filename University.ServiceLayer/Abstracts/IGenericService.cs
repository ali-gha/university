using System.Collections.Generic;
using System.Threading.Tasks;

namespace University.ServiceLayer.Abstracts
{
    public interface IGenericService<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsyncAsNotTracked(int id);
        Task<int> CreateAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T course);
    }
    
}