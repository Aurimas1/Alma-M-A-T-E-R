using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class ReservationRepository : IRepository<Reservation>
    {
        private readonly ApiDbContext context;

        public ReservationRepository(ApiDbContext context)
        {
            this.context = context;
        }

        public async Task<Reservation> Add(Reservation item)
        {
            await context.Reservations.AddAsync(item);
            return context.SaveChanges() == 1 ? item : null;
        }

        public bool Delete(int id)
        {
            context.Reservations.Remove(Get(id));
            return context.SaveChanges() == 1;
        }

        public Reservation Get(int id)
        {
            return context.Reservations.FirstOrDefault(x => x.ReservationID == id);
        }

        public Reservation Get(Func<Reservation, bool> predicate)
        {
            return context.Reservations.FirstOrDefault(predicate);
        }

        public IEnumerable<Reservation> GetAll()
        {
            return context.Reservations.ToList();
        }

        public IEnumerable<Reservation> GetAll(Func<Reservation, bool> predicate)
        {
            return context.Reservations.Where(predicate);
        }

        public Reservation Update(Reservation item)
        {
            context.Reservations.Update(item);
            return context.SaveChanges() == 1 ? item : null;
        }
    }
}
