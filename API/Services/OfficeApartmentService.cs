using System;
using System.Collections.Generic;
using System.Linq;
using API.Constants;
using API.Repositories;

namespace API.Services
{
    public class OfficeApartmentService : IOfficeApartmentService
    {
        private readonly IRepository<Apartment> apartmentsRepository;
        private readonly IRepository<Office> officeRepository;
        private readonly IRepository<Reservation> reservationRepository;
        private readonly TripRepository tripRepository;

        public OfficeApartmentService(IRepository<Apartment> apartmentsRepository, IRepository<Office> officeRepository, IRepository<Reservation> reservationRepository, TripRepository tripRepository)
        {
            this.apartmentsRepository = apartmentsRepository;
            this.officeRepository = officeRepository;
            this.reservationRepository = reservationRepository;
            this.tripRepository = tripRepository;
        }
        
        public IEnumerable<Apartment> GetAll()
        {
            return apartmentsRepository.GetAll();
        }

        public IEnumerable<OfficeAndApartmentsDTO> GetAllOfficeApartments()
        {
            List<Apartment> apartments = apartmentsRepository.GetAll(e => e.Type == ApartmentType.Office).ToList();
            List<OfficeAndApartmentsDTO> officeAndApartmentsDtos = new List<OfficeAndApartmentsDTO>();
            apartments.ForEach(e => officeAndApartmentsDtos.Add(new OfficeAndApartmentsDTO
            {
                ApartmentId = e.ApartmentID,
                Office = officeRepository.Get(e.OfficeId.GetValueOrDefault()).City + ", " + officeRepository.Get(e.OfficeId.GetValueOrDefault()).Country ,
                Name = e.Name,
                Address = e.Address,
                RoomNumber = e.RoomNumber
            }));
            return officeAndApartmentsDtos;
        }

        public IDictionary<int, FreeRooms> GetApartamentOccupationByTrip(int id)
        {
            var trip = tripRepository.Get(id);
            var apartaments = trip.ArrivalOffice.Apartaments;
            var from = trip.DepartureDate;
            var to = trip.ReturnDate;
            
            Dictionary<int, FreeRooms> dict = new Dictionary<int, FreeRooms>();
            foreach (var apar in apartaments)
            {
                dict[apar.RoomNumber] = new FreeRooms()
                {
                    IsRoomIsOccupied = reservationRepository.GetAll(
                        x => x.ApartmentID == apar.ApartmentID &&
                        ((x.CheckIn  > from && x.CheckIn  < to) ||
                         (x.CheckOut > from && x.CheckOut < to) ||
                         (x.CheckIn == from) ||
                         (x.CheckOut == to) ||
                         (x.CheckIn  > from && x.CheckOut < to))).Any(),
                    EmployeeID = reservationRepository.GetAll(x => x.ApartmentID == apar.ApartmentID && x.CheckIn == from && x.CheckOut == to)?.FirstOrDefault()?.EmployeeID,
                };
            }
            return dict;
        }
    }

    public class OfficeAndApartmentsDTO
    {
        public int ApartmentId { get; set; }
        public string Office { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int RoomNumber { get; set; }
    }
}