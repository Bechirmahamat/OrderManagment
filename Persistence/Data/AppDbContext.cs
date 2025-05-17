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



            modelBuilder.Entity<Company>(c =>
            {
                c.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(x => x.ManagerId)
                 .HasPrincipalKey(x => x.Id)
                 .OnDelete(DeleteBehavior.Restrict);

                c.HasMany(x => x.Products)
                 .WithOne(p => p.Company)
                 .OnDelete(DeleteBehavior.Cascade);

                c.HasMany(x => x.Orders)
                 .WithOne(o => o.Company)
                 .HasForeignKey(o => o.CompanyId)  // Explicit FK
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Order>(o =>
            {
                o.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(o => o.UserId)
                 .HasPrincipalKey(u => u.Id)
                 .OnDelete(DeleteBehavior.Restrict);

                o.HasOne(o => o.Company)
                 .WithMany(c => c.Orders)
                 .HasForeignKey(o => o.CompanyId)
                 .OnDelete(DeleteBehavior.Restrict);

                o.HasMany(o => o.OrderItems)
                 .WithOne(oi => oi.Order)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Product>(p =>
            {
                p.HasOne(p => p.Company)
                 .WithMany(c => c.Products)
                 .OnDelete(DeleteBehavior.Cascade);

                p.HasOne(p => p.Category)
                 .WithMany(pc => pc.Products)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
