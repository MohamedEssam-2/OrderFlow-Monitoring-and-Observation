using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderFlow.Application.Abstractions;
using OrderFlow.Application.Monitoring;

namespace OrderFlow.Application.Features.Order.Queries.GetOrders
{
    public class GetOrdersQueryHandler(IApplicationDbContext _context , IMapper _mapper, ILogger<GetOrdersQueryHandler> _logger) : IRequestHandler<GetOrdersQuery, List<OrderListDto>>
    {
        public async Task<List<OrderListDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            using var activity =OrderFlowTracing.ActivitySource.StartActivity("Get Orders");
            var orders = await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.Customer)
                .ToListAsync(cancellationToken);

            activity?.SetTag("orders.count", orders.Count);

            _logger.LogInformation("Retrieved {OrderCount} orders.",orders.Count);

            return _mapper.Map<List<OrderListDto>>(orders);
        }
    
    }
}
