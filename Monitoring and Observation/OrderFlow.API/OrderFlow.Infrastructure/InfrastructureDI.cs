using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderFlow.Application;
using OrderFlow.Application.Abstractions;
using OrderFlow.Infrastructure.BackgroundServices;
using OrderFlow.Infrastructure.Caching;
using OrderFlow.Infrastructure.Database;
using OrderFlow.Infrastructure.Services;

namespace OrderFlow.Infrastructure
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<OrderFlowDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("SqlServer"));
            });
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<OrderFlowDbContext>());
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<IOrderDashboardReadModelService,OrderDashboardReadModelService>();
            services.AddHostedService<OrderDashboardBackgroundService>();
            services.AddScoped<IOrderProcessingService,OrderProcessingService>();
            services.AddHostedService<OrderProcessingBackgroundService>();
            return services;
        }
    }
}
