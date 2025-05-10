using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; private set; }
        public DbSet<ProductCategory> ProductCategories { get; private set; }
        public DbSet<Order> Orders { get; private set; }
        public DbSet<OrderItem> OrderItems { get; private set; }
        public DbSet<Company> Companies { get; private set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Company - Manager (User)
            modelBuilder.Entity<Company>()
                .HasOne(c => c.Manager)
                .WithMany() // Assuming a user can manage many companies, you can change this to WithOne() if only one
                .HasForeignKey(c => c.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Company - Products
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Company)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Company - Orders (via OrderItems)
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Orders)
                .WithOne(oi => oi.Company)
                .HasForeignKey(oi => oi.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order - User
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany() // Add .WithMany(u => u.Orders) if User has Orders
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order - OrderItems
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product - OrderItems
            modelBuilder.Entity<Product>()
                .HasMany<OrderItem>()
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProductCategory - Products
            modelBuilder.Entity<ProductCategory>()
                .HasMany(pc => pc.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
