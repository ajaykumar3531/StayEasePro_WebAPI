using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StayEasePro_Application.CommonRepos.Contracts;
using StayEasePro_Core.Entities;
using System.Linq.Expressions;

namespace StayEasePro_Application.CommonRepos.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly StayeaseproContext _context;
        private readonly DbSet<T> _dbSet;
         

        public BaseRepository(StayeaseproContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> where)
        {
            try
            {
                return await _dbSet.Where(where).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task AddAsync(T entity)
        {

            try
            {
                _dbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task BulkSave(List<T> entities)
        {
            try
            {
                await _context.BulkInsertAsync(entities.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAsync(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var data = await _dbSet.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> GetByIdAsync(object id)
        {
            try
            {
                var data = await _dbSet.FindAsync(id);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
