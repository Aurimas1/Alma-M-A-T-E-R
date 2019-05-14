using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Constants;
using API.Repositories;
using Microsoft.EntityFrameworkCore;

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
                Office = officeRepository.Get(e.OfficeId.GetValueOrDefault()).City + ", " + officeRepository.Get(e.OfficeId.GetValueOrDefault()).Country,
                OfficeId = e.OfficeId.GetValueOrDefault(),
                Name = e.Name,
                Address = e.Address,
                RoomNumber = e.RoomNumber,
                RowVersion = e.RowVersion
            }));
            return officeAndApartmentsDtos;
        }

        public void UpdateApartment(Apartment apartment)
        {
            apartmentsRepository.Update(apartment);
        }

        public void DeleteApartment(int id)
        {
            apartmentsRepository.Delete(id);
        }
        
        public async Task<Apartment> CreateApartment(Apartment apartment)
        {
            return await apartmentsRepository.Add(apartment);
        }
    }

    public class OfficeAndApartmentsDTO
    {
        public int ApartmentId { get; set; }
        public string Office { get; set; }
        public int OfficeId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int RoomNumber { get; set; }
        public byte[] RowVersion { get; set; }
    }
}