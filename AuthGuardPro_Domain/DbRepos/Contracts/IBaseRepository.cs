using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Application.CommonRepos.Contracts
{
    public interface IBaseRepository<T> where T : class
    {
        // Retrieve all records
        Task<IEnumerable<T>> GetAllAsync();

        // Retrieve a record by id
        Task<T> GetByIdAsync(object id);

        // Add a new record
        Task AddAsync(T entity);

        // Update an existing record
        Task UpdateAsync(T entity);

        // Delete a record by id
        Task DeleteAsync(T entity);

        // Save changes to the database
        Task<int> SaveChangesAsync();

        // Bulk update multiple records
        Task BulkSave(List<T> entities);

        Task BulkUpdate(List<T> entities);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> where);
    }
}
