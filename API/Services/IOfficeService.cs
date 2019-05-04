using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IOfficeService
    {
        IEnumerable<Office> GetAll();
        Task<Office> Ensure(Office office);
    }
}
