using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Infrastructure.Database.Configurations
{
    public class OrderDashboardReadModelConfiguration : IEntityTypeConfiguration<OrderDashboardReadModel>
    {
        public void Configure(EntityTypeBuilder<OrderDashboardReadModel> builder)
        {
            builder.ToTable("OrderDashboardReadModels");

            builder.HasKey(x => x.OrderId);

            builder.Property(x => x.OrderId)
                .ValueGeneratedNever();

            builder.Property(x => x.CustomerName)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.ItemCount)
                .IsRequired();

            builder.Property(x => x.Total)
                .HasPrecision(18, 2);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.LastRefreshedAtUtc)
                .IsRequired();
        }
    }
}
