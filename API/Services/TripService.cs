using API.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class TripService : ITripService
    {
        private readonly IRepository<Trip> repository;
        private readonly IRepository<GasCompensation> gasCompensationRepository;
        private readonly IRepository<CarRental> carRentalRepository;
        private readonly IRepository<PlaneTicket> planeTicketRepository;

        public TripService(IRepository<Trip> repository, IRepository<GasCompensation> gasCompensationRepository, IRepository<CarRental> carRentalRepository, IRepository<PlaneTicket> planeTicketRepository)
        {
            this.repository = repository;
            this.gasCompensationRepository = gasCompensationRepository;
            this.carRentalRepository = carRentalRepository;
            this.planeTicketRepository = planeTicketRepository;
        }

        public async Task<GasCompensation> SaveGasCompensation(GasCompensation item)
        {
            return await gasCompensationRepository.Add(item);
        }

        public async Task<CarRental> SaveCarRental(CarRental item)
        {
            return await carRentalRepository.Add(item);
        }

        public async Task<PlaneTicket> SavePlaneTicket(PlaneTicket item)
        {
            return await planeTicketRepository.Add(item);
        }

        public async Task<Trip> Add(Trip item)
        {
            return await repository.Add(item);
        }

        public Trip Update(Trip item)
        {
            return repository.Update(item);
        }

        public IEnumerable<Trip> GetAll()
        {
            return repository.GetAll();
        }
        public Trip GetByID(int id)
        {
            return repository.Get(id);
        }

        public Times GetTimes(int id)
        {
            var trip = repository.Get(id);
            return new Times()
            {
                DepartureDate = trip.DepartureDate,
                ReturnDate = trip.ReturnDate,
            };
        }

        public IEnumerable<Employee> GetEmployees(int id) // need testing
        {
            return repository.Get(id).EmployeesToTrip.Select(x => x.Employee);
        }

        public IEnumerable<Apartment> GetReservedApartments(int id)
        {
            return repository.Get(id).Reservations.Select(x => x.Apartment); // need testing
        }

        public IEnumerable<PlaneTicket> GetPlaneTickets(int id)
        {
            return repository.Get(id).PlaneTickets;
        }

        public IEnumerable<CarRental> GetCarRentals(int id)
        {
            return repository.Get(id).CarRentals;
        }

        public IEnumerable<GasCompensation> GetGasCompensations(int id)
        {
            return repository.Get(id).GasCompensations;
        }









    }
}
