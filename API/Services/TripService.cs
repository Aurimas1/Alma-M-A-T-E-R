using API.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public class TripService : ITripService
    {
        private readonly IRepository<Trip> repository;

        public TripService(IRepository<Trip> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Trip> GetAll()
        {
            return repository.GetAll();
        }
    }
}
