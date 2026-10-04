using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderFlow.Domain.Enums;

namespace OrderFlow.Domain.Entities
{
    public class OrderDashboardReadModel
    {
        public int OrderId { get; set; }

        public string CustomerName { get; set; } = null!;

        public int ItemCount { get; set; }

        public decimal Total { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime LastRefreshedAtUtc { get; set; }
    }
}
