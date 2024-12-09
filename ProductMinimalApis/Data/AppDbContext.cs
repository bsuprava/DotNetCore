using Microsoft.EntityFrameworkCore;
using ProductMinimalApis.Models;
using System.Reflection.Metadata;

namespace ProductMinimalApis.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options) {
        }
        public DbSet<Products> Products { get; set; }
        public DbSet<Categoryies> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }        
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyShopDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Products>().HasKey(p => p.ProductId);    
            modelBuilder.Entity<Categoryies>().HasKey(c => c.CategoryId);
            modelBuilder.Entity<Order>().HasKey(p => p.OrderId);

            modelBuilder.Entity<Categoryies>()                
                        .HasMany(e => e.Products)
                        .WithOne(e => e.Category)
                        .HasForeignKey(e => e.CategoryId)
                        .IsRequired();           

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Products)
                .WithMany();
        }
    }
}
