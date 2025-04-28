using Microsoft.EntityFrameworkCore;
using MotoApp2.Entities;

namespace MotoApp2.Data
{
    public class MotoAppDbContext : DbContext
    {
        public MotoAppDbContext(DbContextOptions<MotoAppDbContext> options)
            : base(options)
        {

        }
        public DbSet<Car> Cars => Set<Car>();
        public DbSet<Customer> Customers => Set<Customer>();

        
    }
}
