using API.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly IRepository<Office> repository;

        public OfficeService(IRepository<Office> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Office> GetAll()
        {
            return repository.GetAll();
        }

        public async Task<Office> Ensure(Office Office)
        {
            var exists1 = repository.Get(e => e.Country == Office.Country);
            var exists2 = repository.Get(e => e.City == Office.City);
            var exists3 = repository.Get(e => e.Address == Office.Address);
            if (!(exists1 == exists2 && exists2 == exists3 && exists1 == exists3))
            {
                return await repository.Add(new Office { Country = Office.Country, City = Office.City, Address = Office.Address });
            }
            else
            {
                return exists1;
            }
        }
    }
}
