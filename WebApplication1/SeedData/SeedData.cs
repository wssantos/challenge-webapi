using ProductApi.Models;
using System.Security.Cryptography;
using System.Text;

namespace ProductApi.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Seed Users
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Username = "admin",
                        Email = "admin@example.com",
                        PasswordHash = HashPassword("admin123")
                    },
                    new User
                    {
                        Username = "user",
                        Email = "user@example.com",
                        PasswordHash = HashPassword("user123")
                    }
                );
            }

            // Seed Products
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        Id = 1, 
                        Name = "Laptop",
                        Description = "High-performance laptop for development",
                        Price = 1299.99m,
                        StockQuantity = 50,
                        Category = "Electronics"
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Wireless Mouse",
                        Description = "Wireless mouse",
                        Price = 29.99m,
                        StockQuantity = 100,
                        Category = "Accessories"
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Travel Book",
                        Description = "Living in Copacabana is awsome!",
                        Price = 59.99m,
                        StockQuantity = 25,
                        Category = "Books"
                    }
                );
            }

            context.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
