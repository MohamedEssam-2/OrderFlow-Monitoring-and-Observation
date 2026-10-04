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

namespace OrderFlow.Application.Features.Order.Queries.GetOrderByid
{
    public class GetOrderByIdQueryHandler(IApplicationDbContext _context, IMapper _mapper,ICacheService _cache, ILogger<GetOrderByIdQueryHandler> _logger) : IRequestHandler<GetOrderByIdQuery, OrderDetailsDto?>
    {
        public async Task<OrderDetailsDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            #region without cacahing
            //var order = await _context.Orders
            //.AsNoTracking()
            //.Include(x => x.Customer)
            //.Include(x => x.Items)
            //.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            //if (order is null)
            //    throw new Exception("Order not found.");

            //return _mapper.Map<OrderDetailsDto>(order);
            #endregion
            using var activity =OrderFlowTracing.ActivitySource.StartActivity("Get Order By ID");
            activity?.SetTag("order.id", request.Id);
            var cacheKey = $"order:{request.Id}";

            // 1. Try cache first
            var cachedOrder =await _cache.GetAsync<OrderDetailsDto>(cacheKey, cancellationToken);
            if (cachedOrder is not null)
            {
                activity?.SetTag("cache.hit", true);
                _logger.LogInformation("Cache hit for OrderId: {OrderId}",request.Id);
                return cachedOrder;
            }

            // 2. Cache miss --> go to database

            activity?.SetTag("cache.hit", false);
            _logger.LogInformation("Cache miss for OrderId: {OrderId}",request.Id);

            var order = await _context.Orders
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Items)
                .FirstOrDefaultAsync( x => x.Id == request.Id,cancellationToken);

            if (order is null)
            {
                activity?.SetTag("order.found", false);

                _logger.LogWarning("Order not found. OrderId: {OrderId}",request.Id);
                return null;
            }
            activity?.SetTag("order.found", true);
            // 3. Map Entity → DTO
            var orderDto =_mapper.Map<OrderDetailsDto>(order);

            // 4. Store result in Redis
            await _cache.SetAsync(cacheKey,orderDto,TimeSpan.FromMinutes(5),cancellationToken);

            // 5. Return response
            return orderDto;
        }
    
    }
}
