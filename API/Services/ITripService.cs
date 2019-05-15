using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface ITripService
    {
        Task<Trip> Add(Trip item);
        IEnumerable<Trip> GetAll();
        Trip GetByID(int id);
        Trip Update(Trip item);
        IEnumerable<Employee> GetEmployees(int id);
        IEnumerable<Apartment> GetReservedApartments(int id);
        IEnumerable<PlaneTicket> GetPlaneTickets(int id);
        IEnumerable<CarRental> GetCarRentals(int id);
        IEnumerable<GasCompensation> GetGasCompensations(int id);
        Times GetTimes(int id);
    }
}
