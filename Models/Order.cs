using System;
using System.Collections.Generic;

namespace EFC.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int SuperMarketId { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual Customer? Customer { get; set; } = null!;
        public virtual Supermarket? SuperMarket { get; set; } = null!;
       
        public virtual ICollection<OrderDetail> 
            OrderDetails { get; set; } = new List<OrderDetail>();
    }
}