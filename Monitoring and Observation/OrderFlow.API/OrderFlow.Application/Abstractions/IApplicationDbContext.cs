using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;



namespace OrderFlow.Application.Abstractions
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; }
        DbSet<Customer> Customers { get; }
        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }
        DbSet<OrderDashboardReadModel> OrderDashboardReadModels { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
