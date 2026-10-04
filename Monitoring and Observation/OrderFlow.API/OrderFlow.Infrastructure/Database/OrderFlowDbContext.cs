using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Abstractions;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Infrastructure.Database
{
    public sealed class OrderFlowDbContext: DbContext, IApplicationDbContext
    {
        public OrderFlowDbContext(DbContextOptions<OrderFlowDbContext> options): base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<OrderDashboardReadModel> OrderDashboardReadModels{get;set;} = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderFlowDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
