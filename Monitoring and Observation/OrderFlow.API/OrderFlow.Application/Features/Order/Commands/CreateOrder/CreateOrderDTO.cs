using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enums;

namespace OrderFlow.Application.Features.Order.Commands.CreateOrder
{
    public class CreateOrderDTO
    {
        public int CustomerId { get; set; }
        public ICollection<CreateOrderItemDTO> Items { get; set; } = new List<CreateOrderItemDTO>();

    }
}
