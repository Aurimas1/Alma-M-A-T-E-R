using API;

namespace Seeder
{
    class Program
    {
        static readonly Office[] Offices = new[]
            {
                new Office
                {
                    Address = "343 W. Erie St. Suite 600",
                    City = "Chicago",
                    Country = "USA",
                },
                new Office
                {
                    Address = "-",
                    City = "Philadelphia",
                    Country = "USA",
                },
                new Office
                {
                    Address = "36 Toronto Street Suite 260",
                    City = "Toronto",
                    Country = "Canada",
                },
                new Office
                {
                    Address = "8 Devonshire Square",
                    City = "London",
                    Country = "United Kingdom",
                },
                new Office
                {
                    Address = "135 Zalgirio g.",
                    City = "Vilnius",
                    Country = "Lithuania",
                },
                new Office
                {
                    Address = "11d. Juozapaviciaus pr.",
                    City = "Kaunas",
                    Country = "Lithuania",
                },
        };

        static void Main(string[] args)
        {
            using (var context = new SeederDbContext())
            {
                context.Offices.RemoveRange(context.Offices);
                context.Offices.AddRange(Offices);
                context.SaveChanges();
            }
        }
    }
}
