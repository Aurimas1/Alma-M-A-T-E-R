using API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class ApartmentService : IApartmentService
    {
        private readonly IRepository<Apartment> repository;

        public ApartmentService(IRepository<Apartment> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Apartment> GetAll()
        {
            return repository.GetAll();
        }

    }
}
