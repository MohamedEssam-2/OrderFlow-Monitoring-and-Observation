using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OrderFlow.Application.Abstractions;

namespace OrderFlow.Application.Features.Customer.Commands.CreateCustomer
{
    internal class CreateCustomeHandler(IApplicationDbContext _context , IMapper _mapper) : IRequestHandler<CreateCustomerCommand, int>
    {
        public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(request.Customer.Name))
            {
                throw new ArgumentException("Customer name is required.");
            }
            var customer = _mapper.Map<OrderFlow.Domain.Entities.Customer>(request.Customer);
             _context.Customers.Add(customer);
            await _context.SaveChangesAsync(cancellationToken);
            return customer.Id;
        }
    }
}
