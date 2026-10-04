using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Application.Features.Order.Queries.GetOrders
{
    public class OrderListDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public DateTime CreatedAtUtc { get; set; }
        public string Status { get; set; } = null!;
        public decimal Total { get; set; }
        public int ItemCount { get; set; }
    }
}
