using API.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public class TripService : ITripService
    {
        private readonly TripRepository repository;
        private readonly IRepository<GasCompensation> gasCompensationRepository;
        private readonly IRepository<CarRental> carRentalRepository;
        private readonly IRepository<PlaneTicket> planeTicketRepository;
        private readonly IRepository<Apartment> apartmentRepository;
        private readonly IRepository<Reservation> reservationRepository;

        public TripService(TripRepository repository, IRepository<GasCompensation> gasCompensationRepository, IRepository<CarRental> carRentalRepository, IRepository<PlaneTicket> planeTicketRepository, IRepository<Apartment> apartmentRepository, IRepository<Reservation> reservationRepository)
        {
            this.repository = repository;
            this.gasCompensationRepository = gasCompensationRepository;
            this.carRentalRepository = carRentalRepository;
            this.planeTicketRepository = planeTicketRepository;
            this.apartmentRepository = apartmentRepository;
            this.reservationRepository = reservationRepository;
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

        public async Task<Apartment> SaveHotelorHome(Apartment item)
        {
            return await apartmentRepository.Add(item);
        }

        public async Task<Reservation> SaveReservation(Reservation item)
        {
            return await reservationRepository.Add(item);
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

        public IEnumerable<Employee> GetEmployees(int id)
        {
            return repository.GetEmployees(id);
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

        public IEnumerable<Apartment> GetReservedApartments(int id)
        {
            return repository.GetReservedApartments(id);
        }

        public IEnumerable<PlaneTicket> GetPlaneTickets(int id)
        {
            return repository.GetPlaneTickets(id);
        }

        public IEnumerable<CarRental> GetCarRentals(int id)
        {
            return repository.GetCarRentals(id);
        }

        public IEnumerable<GasCompensation> GetGasCompensations(int id)
        {
            return repository.GetGasCompensations(id);
        }
    }
}
