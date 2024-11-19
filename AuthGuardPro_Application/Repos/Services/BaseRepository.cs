using AuthGuardPro_Infrastucture.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using StayEasePro_Core.Entities;

namespace AuthGuardPro_Infrastucture.Repository.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T :  class
    {
        private readonly StayeaseproContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(StayeaseproContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {

            try
            {
                this._dbSet.AddAsync(entity);
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
                this._dbSet.Remove(entity);
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
                var data = await this._dbSet.ToListAsync();
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
                var data = await this._dbSet.FindAsync(id);
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
                return await this._context.SaveChangesAsync();
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
                this._dbSet.Update(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
