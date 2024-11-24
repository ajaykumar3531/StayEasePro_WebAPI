using StayEasePro_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Application.CommonRepos.Contracts
{
    public interface IUnitOfWorkService
    {
        IBaseRepository<User> Users { get; }
        IBaseRepository<Property> Properties { get; }
        IBaseRepository<Room> Rooms { get; }
        IBaseRepository<Tenant> Tenants { get; }
        IBaseRepository<Address> Addresses { get; }
        IBaseRepository<City> Cities { get; }
        IBaseRepository<State> State { get; }
        IBaseRepository<Country> Countries { get; }
       



        Task<int> SaveChangesAsync();
    }
}
