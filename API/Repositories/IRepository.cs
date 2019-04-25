using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Repositories
{
    public interface IRepository<T>
    {
        Task<T> Add(T item);

        IEnumerable<T> GetAll();

        T Get(int id);

        T Update(T item);

        bool Delete(int id);
    }
}
