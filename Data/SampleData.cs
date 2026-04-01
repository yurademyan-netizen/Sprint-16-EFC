using System;
using System.Linq;
using EFC.Models;

namespace EFC.Data;

public static class SampleData
{
    public static void Initialize(ShoppingContext context)
    {
        if (!context.Supermarkets.Any())
        {
            context.Supermarkets.AddRange(
                new Supermarket { Name = "ATB", Address = "Ivano-Frankivsk, Vovchynetska st." },
                new Supermarket { Name = "Silpo", Address = "Ivano-Frankivsk, Mazepy st." },
                new Supermarket { Name = "Arsen", Address = "Ivano-Frankivsk, Mykolaichuka st." },
                new Supermarket { Name = "Metro", Address = "Kyiv, Bandery ave." }
            );
            context.SaveChanges();
        }

        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product { Name = "Bread", Price = 25.50 },
                new Product { Name = "Milk", Price = 32.00 },
                new Product { Name = "Eggs (10 pcs)", Price = 55.00 },
                new Product { Name = "Cheese", Price = 250.00 },
                new Product { Name = "Apples", Price = 30.00 },
                new Product { Name = "Coffee", Price = 150.00 }
            );
            context.SaveChanges();
        }

        if (!context.Customers.Any())
        {
            context.Customers.AddRange(
                new Customer { FirstName = "Ivan", LastName = "Franko", Address = "Ivano-Frankivsk", Discount = 5 },
                new Customer { FirstName = "Taras", LastName = "Shevchenko", Address = "Kyiv", Discount = 0 },
                new Customer { FirstName = "Lesya", LastName = "Ukrainka", Address = "Lviv", Discount = 10 },
                new Customer { FirstName = "Stepan", LastName = "Bandera", Address = "Ivano-Frankivsk", Discount = 15 }
            );
            context.SaveChanges();
        }

        if (!context.Orders.Any())
        {
            var arsen = context.Supermarkets.FirstOrDefault(s => s.Name == "Arsen");
            var silpo = context.Supermarkets.FirstOrDefault(s => s.Name == "Silpo");
            var atb = context.Supermarkets.FirstOrDefault(s => s.Name == "ATB");

            var ivan = context.Customers.FirstOrDefault(c => c.LastName == "Franko");
            var taras = context.Customers.FirstOrDefault(c => c.LastName == "Shevchenko");
            var lesya = context.Customers.FirstOrDefault(c => c.LastName == "Ukrainka");

            context.Orders.AddRange(
                new Order { CustomerId = ivan!.Id, SuperMarketId = arsen!.Id, OrderDate = DateTime.Now.AddDays(-5) },
                new Order { CustomerId = taras!.Id, SuperMarketId = silpo!.Id, OrderDate = DateTime.Now.AddDays(-2) },
                new Order { CustomerId = lesya!.Id, SuperMarketId = atb!.Id, OrderDate = DateTime.Now.AddDays(-1) },
                new Order { CustomerId = ivan.Id, SuperMarketId = silpo.Id, OrderDate = DateTime.Now }
            );
            context.SaveChanges();
        }

        if (!context.OrderDetails.Any())
        {
            var orders = context.Orders.ToList();
            var bread = context.Products.FirstOrDefault(p => p.Name == "Bread");
            var milk = context.Products.FirstOrDefault(p => p.Name == "Milk");
            var coffee = context.Products.FirstOrDefault(p => p.Name == "Coffee");
            var cheese = context.Products.FirstOrDefault(p => p.Name == "Cheese");

            context.OrderDetails.AddRange(
                new OrderDetail { OrderId = orders[0].Id, ProductId = bread!.Id, Quantity = 2 },
                new OrderDetail { OrderId = orders[0].Id, ProductId = milk!.Id, Quantity = 1.5 },
                new OrderDetail { OrderId = orders[1].Id, ProductId = coffee!.Id, Quantity = 1 },
                new OrderDetail { OrderId = orders[2].Id, ProductId = cheese!.Id, Quantity = 0.5 },
                new OrderDetail { OrderId = orders[2].Id, ProductId = bread.Id, Quantity = 1 },
                new OrderDetail { OrderId = orders[3].Id, ProductId = milk.Id, Quantity = 3 }
            );
            context.SaveChanges();
        }
    }
}