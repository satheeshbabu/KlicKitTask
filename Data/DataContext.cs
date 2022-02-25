using KlicKitApi.Models;
using Microsoft.EntityFrameworkCore;

namespace KlicKitApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {           
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<UserProducts> UserProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProducts>().HasKey(up => new {up.UserId, up.ProductId});
        }
        
    }
}