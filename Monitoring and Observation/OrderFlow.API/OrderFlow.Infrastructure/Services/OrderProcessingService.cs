using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderFlow.Application;
using OrderFlow.Application.Abstractions;
using OrderFlow.Application.Monitoring;
using OrderFlow.Domain.Enums;

namespace OrderFlow.Infrastructure.Services
{
    public class OrderProcessingService(IApplicationDbContext _context ,ICacheService _cacheService, OrderFlowMetrics _metrics, ILogger<OrderProcessingService> _logger) : IOrderProcessingService
    {
        public async Task ProcessPendingOrdersAsync(CancellationToken cancellationToken)
        {
            var orders = await _context.Orders
                .Where(x => x.Status == OrderStatus.Pending)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Background worker found {PendingOrderCount} pending orders.",orders.Count);

            foreach (var order in orders)
            {
                order.Status = OrderStatus.Completed;
                _logger.LogInformation("Background worker completed OrderId: {OrderId}.",order.Id);

            }

            await _context.SaveChangesAsync(cancellationToken);

            foreach (var order in orders)
            {
                await _cacheService.RemoveAsync($"order:{order.Id}",cancellationToken);
            }
            _metrics.SetPendingOrders(0);
        }
    }
}
