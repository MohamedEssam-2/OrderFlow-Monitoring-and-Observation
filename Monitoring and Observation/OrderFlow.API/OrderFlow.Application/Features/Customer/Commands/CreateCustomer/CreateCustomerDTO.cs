using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Features.Customer.Commands.CreateCustomer
{
    public class CreateCustomerDTO
    {
        public string Name { get; set; } = null!;
    }
}
