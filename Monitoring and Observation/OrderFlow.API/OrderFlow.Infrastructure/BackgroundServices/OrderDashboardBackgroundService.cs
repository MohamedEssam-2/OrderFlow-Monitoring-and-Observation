using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OrderFlow.Application.Abstractions;
using Microsoft.Extensions.Hosting;
namespace OrderFlow.Infrastructure.BackgroundServices
{
    public class OrderDashboardBackgroundService(IServiceScopeFactory _scopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using var scope = _scopeFactory.CreateScope();

                    var service = scope.ServiceProvider.GetRequiredService<IOrderDashboardReadModelService>();

                    await service.RefreshAsync(stoppingToken);
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("BACKGROUND SERVICE ERROR:");
                Console.WriteLine(ex.ToString());
            }


        }
    }
}
