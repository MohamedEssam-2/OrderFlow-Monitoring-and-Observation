using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace OrderFlow.Application.Features.Customer.Commands.CreateCustomer
{
    public record CreateCustomerCommand(CreateCustomerDTO Customer) : IRequest<int>
    {
       
    }
}
