using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IOfficeApartmentService
    {
        IEnumerable<Apartment> GetAll();
        IEnumerable<OfficeAndApartmentsDTO> GetAllOfficeApartments();
        void UpdateApartment(Apartment apartment);
        void DeleteApartment(int id);
        Task<Apartment> CreateApartment(Apartment apartment);
        IDictionary<int, FreeRooms> GetApartamentOccupationByTrip(int id);
    }
}