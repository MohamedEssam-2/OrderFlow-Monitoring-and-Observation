using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderFlow.Domain.Enums;

namespace OrderFlow.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public DateTime CreatedAtUtc { get; set; }= DateTime.UtcNow;

        public OrderStatus Status { get; set; }= OrderStatus.Pending;

        public decimal Total { get; set; }

        public ICollection<OrderItem> Items { get; set; }= new List<OrderItem>();
    }
}
