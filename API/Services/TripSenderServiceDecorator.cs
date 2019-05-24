using API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public class TripSenderServiceDecorator : ITripService
    {
        private readonly ITripService service;
        private readonly IMailSender sender;
        private readonly IEmployeeService employee;

        public TripSenderServiceDecorator(ITripService service, IMailSender sender, IEmployeeService employee)
        {
            this.service = service;
            this.sender = sender;
            this.employee = employee;
        }

        public Task<GasCompensation> SaveGasCompensation(GasCompensation item)
        {
            return service.SaveGasCompensation(item);
        }

        public Task<CarRental> SaveCarRental(CarRental item)
        {
            return service.SaveCarRental(item);
        }

        public Task<PlaneTicket> SavePlaneTicket(PlaneTicket item)
        {
            return service.SavePlaneTicket(item);
        }

        public Task<Apartment> SaveHotelorHome(Apartment item)
        {
            return service.SaveHotelorHome(item);
        }

        public bool Delete(int id)
        {
            return service.Delete(id);
        }

        public Task<Reservation> SaveReservation(Reservation item)
        {
            return service.SaveReservation(item);
        }

        public async Task<Trip> Add(Trip item)
        {
            var result = await service.Add(item);
            if (result.OrganizerID.HasValue) {
                var org = employee.Get(result.OrganizerID ?? 0);
                sender.Send(org.Email, org.Name, "Trip was created", "yey");
            }
            return result;
        }

        public Trip Update(Trip item)
        {
            return service.Update(item);
        }

        public IEnumerable<Trip> GetAll()
        {
            return service.GetAll();
        }

        public IEnumerable<Trip> GetAllMyTrips()
        {
            return service.GetAllMyTrips();
        }

        public Trip GetByID(int id)
        {
            return service.GetByID(id);
        }

        public TimeAndTransport GetTimeAndTransport(int id)
        {
            return service.GetTimeAndTransport(id);
        }

        public IEnumerable<EmployeeWithStatus> GetEmployees(int id)
        {
            return service.GetEmployees(id);
        }

        public IEnumerable<Apartment> GetReservedApartments(int id)
        {
            return service.GetReservedApartments(id);
        }

        public IEnumerable<PlaneTicket> GetPlaneTickets(int id)
        {
            return service.GetPlaneTickets(id);
        }

        public IEnumerable<CarRental> GetCarRentals(int id)
        {
            return service.GetCarRentals(id);
        }

        public IEnumerable<GasCompensation> GetGasCompensations(int id)
        {
            return service.GetGasCompensations(id);
        }

        public Trip Get(int id)
        {
            return service.Get(id);
        }

        public IEnumerable<Trip> GetYourOrganizedTrips() {
            return service.GetYourOrganizedTrips();
        }

        public IEnumerable<Trip> GetOtherOrganizedTrips()
        {
            return service.GetOtherOrganizedTrips();
        }
    }
}
