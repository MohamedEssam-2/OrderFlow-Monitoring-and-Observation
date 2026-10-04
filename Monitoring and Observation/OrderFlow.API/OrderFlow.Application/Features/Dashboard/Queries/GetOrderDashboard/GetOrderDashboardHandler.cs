using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderFlow.Application.Abstractions;

namespace OrderFlow.Application.Features.Dashboard.Queries.GetOrderDashboard
{
    public class GetOrderDashboardHandler(IApplicationDbContext _context ) : IRequestHandler<GetOrderDashboardQuery, List<OrderDashboardDto>>
    {
        public async Task<List<OrderDashboardDto>> Handle(GetOrderDashboardQuery request, CancellationToken cancellationToken)
        {
            return await _context.OrderDashboardReadModels
            .AsNoTracking()
            .Select(x => new OrderDashboardDto
            {
                OrderId = x.OrderId,
                CustomerName = x.CustomerName,
                ItemCount = x.ItemCount,
                Total = x.Total,
                Status = x.Status.ToString(),
            })
            .ToListAsync(cancellationToken);
        }
    }
}
