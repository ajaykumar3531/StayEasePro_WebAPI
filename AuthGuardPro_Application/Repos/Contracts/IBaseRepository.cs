using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthGuardPro_Infrastucture.Repository.Contracts
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
    }
}
