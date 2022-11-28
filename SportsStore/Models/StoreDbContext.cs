using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models{
    public class StoredDbContext : DbContext 
    {
        public StoredDbContext(DbContextOptions<StoredDbContext> options): base(options)
        {

        }
        public DbSet<Product> Products{get;set;}
        public DbSet<Order> Orders{get;set;}
    }
}