using Microsoft.EntityFrameworkCore;
using MotoApp2.Entities;

namespace MotoApp2.Data
{
    public class MotoAppDbContext : DbContext
    {
        public DbSet<Cars> Cars => Set<Cars>();
        public DbSet<Customer> Customers => Set<Customer>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("StorageAppDb");
        }
    }
}
