using System;
using System.Collections.Generic;

namespace API.Services
{
    public interface IOfficeApartmentService
    {
        IEnumerable<Apartment> GetAll();
        IEnumerable<OfficeAndApartmentsDTO> GetAllOfficeApartments();
        IDictionary<int, FreeRooms> GetApartamentOccupationByOffice(int id, DateTime from, DateTime to);
    }
}