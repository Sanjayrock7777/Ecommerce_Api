using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Data
{
    public class EcomDbContext: IdentityDbContext<ApplicationUser>
    {
        public EcomDbContext(DbContextOptions<EcomDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }  
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        //public DbSet<Payment> Payments { get; set; } 
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    builder.Entity<Customer>().HasOne(c => c.User).WithOne(u => u.Customer).
        //        HasForeignKey<Customer>(c => c.UserId);

        //    builder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products).
        //        HasForeignKey(p => p.CategoryId);
        //}

    }
 
}
