using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MarkRestaurant.Models;

namespace MarkRestaurant.Data
{
    public class MarkRestaurantDbContext : IdentityDbContext<User>
    {
        public MarkRestaurantDbContext(DbContextOptions<MarkRestaurantDbContext> options) : base(options)
        {
            Database.EnsureCreated();
            if (!_menuProducts.Any())
                SeedData();
        }

        public DbSet<Admin> Admin => Set<Admin>();
        public DbSet<User> _users => Set<User>();
        public DbSet<Product> _menuProducts => Set<Product>();
        public DbSet<Order> _orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .Property(b => b.UserId)
                .IsRequired();
        }
        public void SeedData()
        {
            var products = new List<Product> {
                new Product { Title = "Double Big Tasty(beef)", Category = "Burgers", Price = 13.75, ImageUrl = "/images/bigtasty.jpg" },
                new Product { Title = "Double Big Tasty(chicken)", Category = "Burgers", Price = 13.75, ImageUrl = "/images/bigtasty.jpg" },
                new Product { Title = "Hamburger", Category = "Burgers", Price = 2.70, ImageUrl = "/images/hamburger.jpg" },
                new Product { Title = "Double Hamburger", Category = "Burgers", Price = 4.30, ImageUrl = "/images/hamburger.jpg" },
                new Product { Title = "Triple Hamburger", Category = "Burgers", Price = 6.00, ImageUrl = "/images/hamburger.jpg" },
                new Product { Title = "Cheeseburger", Category = "Burgers", Price = 2.95, ImageUrl = "/images/cheeseburger.jpg" },
                new Product { Title = "Double Cheeseburger", Category = "Burgers", Price = 5.40, ImageUrl = "/images/cheeseburger.jpg" },
                new Product { Title = "Triple Cheeseburger", Category = "Burgers", Price = 7.25, ImageUrl = "/images/cheeseburger.jpg" },

                new Product { Title = "French Frize(Extra)", Category = "Potato", Price = 5.70, ImageUrl = "/images/frize.jpg" },
                new Product { Title = "French Frize(Big)", Category = "Potato", Price = 5.20, ImageUrl = "/images/frize.jpg" },
                new Product { Title = "French Frize(Average)", Category = "Potato", Price = 4.30, ImageUrl = "/images/frize.jpg" },
                new Product { Title = "French Frize(Small)", Category = "Potato", Price = 3.00, ImageUrl = "/images/frize.jpg" },
                new Product { Title = "Rustic potatoes(Extra)", Category = "Potato", Price = 6.30, ImageUrl = "/images/rustic.jpg" },
                new Product { Title = "Rustic potatoes(Big)", Category = "Potato", Price = 5.70, ImageUrl = "/images/rustic.jpg" },
                new Product { Title = "Rustic potatoes(Average)", Category = "Potato", Price = 5.20, ImageUrl = "/images/rustic.jpg" },
                new Product { Title = "Rustic potatoes(Small)", Category = "Potato", Price = 4.30, ImageUrl = "/images/rustic.jpg" },

                new Product { Title = "Chicken Strips(5 p)", Category = "Snacks", Price = 6.50, ImageUrl = "/images/strips.jpg" },
                new Product { Title = "Chicken Strips(3 p)", Category = "Snacks", Price = 4.30, ImageUrl = "/images/strips.jpg" },
                new Product { Title = "Chicken wings(8 p)", Category = "Snacks", Price = 10.50, ImageUrl = "/images/wings.jpg" },
                new Product { Title = "Chicken wings(5 p)", Category = "Snacks", Price = 7.20, ImageUrl = "/images/wings.jpg" },
                new Product { Title = "Chicken wings(3 p)", Category = "Snacks", Price = 4.95, ImageUrl = "/images/wings.jpg" },
                new Product { Title = "Shrimp(6 p)", Category = "Snacks", Price = 10.95, ImageUrl = "/images/shrimp.jpg" },
                new Product { Title = "Shrimp(4 p)", Category = "Snacks", Price = 7.60, ImageUrl = "/images/shrimp.jpg" },
                new Product { Title = "Chicken Box", Category = "Snacks", Price = 18.60, ImageUrl = "/images/box.jpg" },

                new Product { Title = "Coca-Cola 750ml", Category = "Drinks", Price = 4.30, ImageUrl = "/images/cola750.jpg" },
                new Product { Title = "Coca-Cola 500ml", Category = "Drinks", Price = 3.75, ImageUrl = "/images/cola750.jpg" },
                new Product { Title = "Coca-Cola 400ml", Category = "Drinks", Price = 2.95, ImageUrl = "/images/cola750.jpg" },
                new Product { Title = "Coca-Cola 250ml", Category = "Drinks", Price = 2.40, ImageUrl = "/images/cola750.jpg" },
                new Product { Title = "Fanta 750ml", Category = "Drinks", Price = 4.30, ImageUrl = "/images/fanta750.jpg" },
                new Product { Title = "Fanta 500ml", Category = "Drinks", Price = 3.75, ImageUrl = "/images/fanta750.jpg" },
                new Product { Title = "Fanta 400ml", Category = "Drinks", Price = 2.95, ImageUrl = "/images/fanta750.jpg" },
                new Product { Title = "Fanta 250ml", Category = "Drinks", Price = 2.40, ImageUrl = "/images/fanta750.jpg" }
            };
            
            _menuProducts.AddRange(products);
            SaveChanges();
        }
    }
}
