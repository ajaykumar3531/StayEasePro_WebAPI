using Microsoft.EntityFrameworkCore;
using StayEasePro_Application.CommonRepos.Contracts;
using StayEasePro_Application.CommonRepos.Services;
using StayEasePro_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Core.DbRepos.Services
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly StayeaseproContext _context;

        public UnitOfWorkService(StayeaseproContext context)
        {
            _context = context;
        }

        public IBaseRepository<User> Users => new BaseRepository<User>(_context);
        public IBaseRepository<Property> Properties => new BaseRepository<Property>(_context);
        public IBaseRepository<Room> Rooms => new BaseRepository<Room>(_context);
        public IBaseRepository<Tenant> Tenants => new BaseRepository<Tenant>(_context);
        public IBaseRepository<Address> Addresses => new BaseRepository<Address>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
