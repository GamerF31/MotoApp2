using Microsoft.EntityFrameworkCore;
using MotoApp2.Entities;

namespace MotoApp2.Data
{
    public class MotoAppDbContext : DbContext
    {
        public DbSet<Car> Cars => Set<Car>();
        public DbSet<Customer> Customers => Set<Customer>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("StorageAppDb");
        }
    }
}
