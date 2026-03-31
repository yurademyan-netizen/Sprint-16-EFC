using System.Collections.Generic;

namespace EFC.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int Discount { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}