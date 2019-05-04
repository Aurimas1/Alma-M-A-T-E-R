using System.Collections.Generic;

namespace API.Services
{
    public interface ITripService
    {
        IEnumerable<Trip> GetAll();
    }
}
