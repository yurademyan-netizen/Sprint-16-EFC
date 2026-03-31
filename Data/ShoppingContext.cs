using EFC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EFC.Data
{
    public class ShoppingContext : DbContext
    {
        public ShoppingContext(
            DbContextOptions<ShoppingContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Supermarket> Supermarkets { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
    }
}