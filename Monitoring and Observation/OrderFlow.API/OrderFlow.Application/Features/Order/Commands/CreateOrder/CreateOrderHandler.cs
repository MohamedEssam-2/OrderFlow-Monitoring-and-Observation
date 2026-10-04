using System.Diagnostics;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderFlow.Application.Abstractions;
using OrderFlow.Application.Monitoring;
using OrderFlow.Domain.Enums;

namespace OrderFlow.Application.Features.Order.Commands.CreateOrder
{
    public class CreateOrderHandler(IApplicationDbContext _context,IMapper _mapper,OrderFlowMetrics metrics,ILogger<CreateOrderHandler> _logger): IRequestHandler<CreateOrderCommand, int>
    {
        public async Task<int> Handle(CreateOrderCommand request,CancellationToken cancellationToken)
        {
            // Start tracing
            using var activity =OrderFlowTracing.ActivitySource.StartActivity("Create Order");
            activity?.SetTag("customer.id",request.OrderDto.CustomerId);
            activity?.SetTag("order.items_count",request.OrderDto.Items?.Count ?? 0);

            var dto = request.OrderDto;
            var customerExists = await _context.Customers.AnyAsync(x => x.Id == dto.CustomerId,cancellationToken);

            if (!customerExists)
            {
                _logger.LogWarning("Order creation failed. CustomerId {CustomerId} was not found.",dto.CustomerId);
                throw new ArgumentException("Customer not found.");
            }

            if (dto.Items is null || dto.Items.Count == 0)
            {
                _logger.LogWarning("Order creation failed. CustomerId {CustomerId} submitted an order with no items.",dto.CustomerId);
                throw new ArgumentException("Order must contain at least one item.");
            }

            if (dto.Items.Any(x => x.Quantity <= 0))
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            var productIds = dto.Items
                .Select(x => x.ProductId)
                .Distinct()
                .ToList();

            var products = await _context.Products
                .Where(x =>productIds.Contains(x.Id) &&x.IsActive)
                .ToListAsync(cancellationToken);

            if (products.Count != productIds.Count)
            {
                throw new ArgumentException("One or more products are invalid or inactive.");
            }

            var order = new Domain.Entities.Order
            {
                CustomerId = dto.CustomerId,
                CreatedAtUtc = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            foreach (var itemDto in dto.Items)
            {
                var product = products.FirstOrDefault(x => x.Id == itemDto.ProductId);
                var orderItem = new Domain.Entities.OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                };
                order.Items.Add(orderItem);
            }

            order.Total = order.Items.Sum(x => x.Quantity * x.UnitPrice);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            // Add information to trace after order is created
            activity?.SetTag("order.id", order.Id);
            activity?.SetTag("order.total", order.Total);

            metrics.OrdersCreated.Add(1);

            _logger.LogInformation("Order created successfully. OrderId: {OrderId}, CustomerId: {CustomerId}, Total: {Total}",
                order.Id,
                order.CustomerId,
                order.Total);

            return order.Id;
        }
    }
}