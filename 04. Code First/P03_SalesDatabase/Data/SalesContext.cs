using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Store> Stores { get; set; }

        public SalesContext() { }

        public SalesContext(DbContextOptions options)
          : base(options) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DIMA\\SQLEXPRESS;Database=Sales;Integrated Security=True;");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(s => s.SaleId);

                entity.Property(s => s.Date)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasOne(c => c.Customer)
                    .WithMany(s=>s.Sales);

                entity.HasOne(c => c.Product)
                    .WithMany(s => s.Sales);

                entity.HasOne(c => c.Store)
                  .WithMany(s => s.Sales);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(s => s.CustomerId);

                entity.Property(c => c.Name)
                    .HasMaxLength(100)
                    .IsUnicode();

                entity.Property(c => c.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(s => s.ProductId);

                entity.Property(c => c.Name)
                    .HasMaxLength(50)
                    .IsUnicode();

                entity.Property(c => c.Desciption)
                    .HasMaxLength(250)
                    .HasDefaultValue("No description");
            });


            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(s => s.StoreId);

                entity.Property(c => c.Name)
                    .HasMaxLength(80)
                    .IsUnicode();
            });

        }
    }
}
