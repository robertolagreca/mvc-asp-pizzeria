using Microsoft.EntityFrameworkCore;
using MVC_ASP_pizzeria.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MVC_ASP_pizzeria.Database
{
    public class PizzaContext : DbContext
    {

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Database=MVC-ASP-pizzeria-v1;" +
            "Integrated Security=True;TrustServerCertificate=True");
        }
    }
}
