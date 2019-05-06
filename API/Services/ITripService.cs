using System.Collections.Generic;

namespace API.Services
{
    public interface ITripService
    {
        IEnumerable<Trip> GetAll();

        IEnumerable<Employee> GetEmployees(int id);
        IEnumerable<Apartment> GetReservedApartments(int id);
        IEnumerable<PlaneTicket> GetPlaneTickets(int id);
        IEnumerable<CarRental> GetCarRentals(int id);
        IEnumerable<GasCompensation> GetGasCompensations(int id);
    }
}
