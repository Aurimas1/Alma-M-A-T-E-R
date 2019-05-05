using API.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public class TripService : ITripService
    {
        private readonly TripRepository repository;

        public TripService(TripRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Trip> GetAll()
        {
            return repository.GetAll();
        }

        public IEnumerable<Employee> GetEmployeesFromTrip(int id)
        {
            return repository.GetEmployeesFromTrip(id);
        }
    }
}
