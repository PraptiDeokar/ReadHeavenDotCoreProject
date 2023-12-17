
using Project.Models;
using Microsoft.EntityFrameworkCore;
using Project.Models.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;

namespace Project.DataAccess.Data
{
    public class ApplicationDBContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {
                
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Company>Company { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<OrderHeader> OrderHeaders { get; set; }

         public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category {ID=1,Name="Maths",Display_Order=1 },
                   new Category { ID = 2, Name = "Science", Display_Order = 2 }
                );

            modelBuilder.Entity<Company>().HasData(
               new Company { ID = 1, Name = "Tech Mahindra", StreetAddress = "21 sector",City="Pune",PostalCode="412212",
               State="MH",PhoneNumber="9988776655" }     );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ID = 1,
                    Title = "The Power of Habit: Why We Do What We Do in Life and Business",
                    Author = "Charles Duhigg",
                    Description = "This book explores the science behind habit formation and provides valuable insights on how to change and build habits for personal and professional success. ",
                    ISBN = " 978-0812981605",
                    ListPrice = 100,
                    Price = 95,
                    Price50 = 90,
                    Price100 = 85,
                    CategoryId= 1,
                    ImageUrl=""
                },

                new Product
                {
                    ID = 2,
                    Title = "Atomic Habits: An Easy & Proven Way to Build Good Habits & Break Bad Ones",
                    Author = "James Clear",
                    Description = "James Clear's book focuses on the power of small changes and how they can lead to significant personal development. It offers practical strategies for building positive habits and breaking destructive ones. ",
                    ISBN = "978-0735211292",
                    ListPrice = 99,
                    Price = 95,
                    Price50 = 90,
                    Price100 = 85,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                 new Product
                 {
                     ID = 3,
                     Title = "How to Win Friends and Influence People",
                     Author = "Dale Carnegie",
                     Description = " A timeless classic, this book offers valuable advice on building meaningful relationships, improving communication, and becoming more influential in both personal and professional settings.",
                    ISBN = "978-0671027032",
                     ListPrice = 90,
                     Price = 88,
                     Price50 = 85,
                     Price100 = 83,
                     CategoryId = 2,
                     ImageUrl = ""
                 }

                );

           
        }
    }
}
