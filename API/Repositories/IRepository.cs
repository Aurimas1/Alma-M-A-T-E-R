using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Repositories
{
    public interface IRepository<T>
    {
        Task<T> Add(T item);

        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Func<T, bool> predicate);

        T Get(int id);
        T Get(Func<T, bool> predicate);

        T Update(T item);

        bool Delete(int id);
    }
}
