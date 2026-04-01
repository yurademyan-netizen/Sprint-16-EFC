using EFC.Models;
using System.Linq;

namespace EFC.Data
{
    public static class SampleData
    {
        public static void Initialize(ShoppingContext context)
        {
            if (!context.Supermarkets.Any())
            {
                context.Supermarkets.AddRange(
                    new Supermarket { Name = "ATB", Address = "Lviv, Bandery st." },
                    new Supermarket { Name = "Silpo", Address = "Kyiv, Khreshchatyk" }
                );
                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product { Name = "Bread", Price = 25.50 },
                    new Product { Name = "Milk", Price = 32.00 }
                );
                context.SaveChanges();
            }

            if (!context.Customers.Any())
            {
                context.Customers.AddRange(
                    new Customer { FirstName = "Ivan", LastName = "Franko", Address = "Lviv" },
                    new Customer { FirstName = "Taras", LastName = "Shevchenko", Address = "Kyiv" }
                );
                context.SaveChanges();
            }
        }
    }
}