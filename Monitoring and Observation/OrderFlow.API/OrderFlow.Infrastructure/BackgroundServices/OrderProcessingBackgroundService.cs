using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderFlow.Application.Abstractions;
using OrderFlow.Application.Monitoring;

namespace OrderFlow.Infrastructure.BackgroundServices
{
    public class OrderProcessingBackgroundService(IServiceScopeFactory scopeFactory, OrderFlowMetrics metrics) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope =scopeFactory.CreateScope();

                var service =scope.ServiceProvider.GetRequiredService<IOrderProcessingService>();

                metrics.WorkerExecutions.Add(1);

                await service.ProcessPendingOrdersAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(30),stoppingToken);
            }
        }

    }
}
