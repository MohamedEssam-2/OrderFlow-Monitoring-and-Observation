using OrderFlow.Application.Features.Product.Commands.CreateProduct;
using OrderFlow.Infrastructure;
using OrderFlow.Application;
using Microsoft.Extensions.DependencyInjection;
using OrderFlow.Application.Features.Product;
using OrderFlow.Application.Monitoring;
using OpenTelemetry.Metrics;
using Serilog;
using OrderFlow.API.Middleware;
using OpenTelemetry.Trace;
using OrderFlow.Application.Abstractions;
using OrderFlow.Infrastructure.Database;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
namespace OrderFlow.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
             Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(builder.Configuration)
              .CreateLogger();

            builder.Host.UseSerilog();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration =builder.Configuration["Redis:ConnectionString"];
            });

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly);
            });

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(ProductProfile).Assembly); 
            });


            builder.Services.AddSingleton<OrderFlowMetrics>();
            builder.Services
            .AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddMeter(OrderFlowMetrics.MeterName)
                     .AddPrometheusExporter();
            })
                .WithTracing(tracing =>
                {
                    tracing
                     .AddAspNetCoreInstrumentation()
                     .AddSource("OrderFlow")
                     .AddOtlpExporter(options =>
                     {
                         options.Endpoint = new Uri("http://localhost:4317");
                     });

                });
            builder.Services
                .AddHealthChecks()
                .AddDbContextCheck<OrderFlowDbContext>("sqlserver")
                .AddRedis(
                    builder.Configuration["Redis:ConnectionString"]!,
                    name: "redis");



            var app = builder.Build();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapPrometheusScrapingEndpoint();
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";

                    var response = new
                    {
                        status = report.Status.ToString(),

                        checks = report.Entries.Select(x => new
                        {
                            name = x.Key,
                            status = x.Value.Status.ToString(),
                            description = x.Value.Description,
                            duration = x.Value.Duration.TotalMilliseconds
                        })
                    };

                    await context.Response.WriteAsJsonAsync(response);
                }
            });
            app.MapControllers();

            app.Run();
        }
    }
}
