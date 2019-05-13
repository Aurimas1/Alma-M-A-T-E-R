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

        public OfficeApartmentService(IRepository<Apartment> apartmentsRepository, IRepository<Office> officeRepository)
        {
            this.apartmentsRepository = apartmentsRepository;
            this.officeRepository = officeRepository;
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