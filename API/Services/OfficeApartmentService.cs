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

        public OfficeApartmentService(IRepository<Apartment> apartmentsRepository, IRepository<Office> officeRepository, IRepository<Reservation> reservationRepository)
        {
            this.apartmentsRepository = apartmentsRepository;
            this.officeRepository = officeRepository;
            this.reservationRepository = reservationRepository;
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

        public IDictionary<int, bool> GetApartamentOccupationByOffice(int id, DateTime from, DateTime to)
        {
            var apartaments = apartmentsRepository.GetAll(x => x.OfficeId == id);
            Dictionary<int, bool> dict = new Dictionary<int, bool>();
            foreach(var apar in apartaments)
            {
                dict[apar.RoomNumber] = reservationRepository.GetAll(x => x.ApartmentID == apar.ApartmentID && ((x.CheckOut > from || x.CheckIn > from || x.CheckIn < to || x.CheckOut < to) || (x.CheckIn > from && x.CheckOut < to))).Any();
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