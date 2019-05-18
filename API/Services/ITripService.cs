using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface ITripService
    {
        Task<Trip> Add(Trip item);
        IEnumerable<Trip> GetAll();
        Task<GasCompensation> SaveGasCompensation(GasCompensation item);
        Task<CarRental> SaveCarRental(CarRental item);
        Task<PlaneTicket> SavePlaneTicket(PlaneTicket item);
        Task<Apartment> SaveHotelorHome(Apartment item);
        Task<Reservation> SaveReservation(Reservation item);
        Trip GetByID(int id);
        Trip Update(Trip item);
        IEnumerable<Employee> GetEmployees(int id);
        IEnumerable<Apartment> GetReservedApartments(int id);
        IEnumerable<PlaneTicket> GetPlaneTickets(int id);
        IEnumerable<CarRental> GetCarRentals(int id);
        IEnumerable<GasCompensation> GetGasCompensations(int id);
        Trip Get(int id);
        Times GetTimes(int id);
        IEnumerable<Trip> GetYourOrganizedTrips();
        IEnumerable<Trip> GetOtherOrganizedTrips();
    }
}
