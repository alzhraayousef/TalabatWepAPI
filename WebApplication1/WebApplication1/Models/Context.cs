using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class Context:IdentityDbContext<ApplicationUser>
    {
        
        public Context(DbContextOptions options):base(options) 
        { }

        public Context() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-KHKE8CC\SQL19;Initial Catalog=Talabat;Integrated Security=True;Encrypt=False");
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Address> Address { get; set; }
       public DbSet<Cart> Cart { get; set; }
       public DbSet<Category> Category { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<District> District { get; set; }
        public DbSet<Offer> Offer { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderProducts> OrderProducts { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Resturant> Resturant { get; set; }
        public DbSet<ResturantCategories> ResturantCategories { get; set; }
        public DbSet<ResturantCities> ResturantCities { get; set; }
        public DbSet<Review> Review { get; set; }

    }
}
