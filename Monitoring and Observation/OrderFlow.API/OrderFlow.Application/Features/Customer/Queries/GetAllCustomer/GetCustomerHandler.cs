using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Abstractions;
using OrderFlow.Application.Features.Product.Queries.GetAllProducts;

namespace OrderFlow.Application.Features.Customer.Queries.GetAllCustomer
{
    public class GetCustomerHandler(IApplicationDbContext _context, IMapper _mapper) : IRequestHandler<GetCustomerQuery, List<CustomerDTO>>
    {
        public async Task<List<CustomerDTO>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            return await _context.Customers
                .AsNoTracking()
                .ProjectTo<CustomerDTO>(
                    _mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
