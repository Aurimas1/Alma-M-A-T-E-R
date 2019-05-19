using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IApartmentService
    {
        IEnumerable<Apartment> GetAll();
        Apartment Get(int id);
        Apartment Update(Apartment item);
    }
}