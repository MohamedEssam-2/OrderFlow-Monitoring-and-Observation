using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Abstractions;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Infrastructure.Services
{
    public class OrderDashboardReadModelService(IApplicationDbContext _context) : IOrderDashboardReadModelService
    {
        public async Task RefreshAsync(CancellationToken cancellationToken = default)
        {
            var orders = await _context.Orders
           .AsNoTracking()
           .Include(x => x.Customer)
           .Include(x => x.Items)
           .ToListAsync(cancellationToken);

            foreach (var order in orders)
            {
                var readModel =await _context.OrderDashboardReadModels.FirstOrDefaultAsync(x => x.OrderId == order.Id,cancellationToken);

                if (readModel is null)
                {
                    readModel = new OrderDashboardReadModel
                    {
                        OrderId = order.Id
                    };

                    _context.OrderDashboardReadModels.Add(readModel);
                }

                readModel.CustomerName = order.Customer.Name;
                readModel.ItemCount = order.Items.Sum(x => x.Quantity);
                readModel.Total = order.Total;
                readModel.Status = order.Status;
                readModel.LastRefreshedAtUtc = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
    
}
