using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OrderFlow.Application.Features.Order.Commands.CreateOrder;
using OrderFlow.Application.Features.Order.Queries.GetOrderByid;
using OrderFlow.Domain.Entities;
using OrderFlow.Application.Features.Order.Queries.GetOrders;
namespace OrderFlow.Application.Features.Order
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderFlow.Domain.Entities.Order, OrderDetailsDto>()
                 .ForMember(dest => dest.CustomerName,opt => opt.MapFrom(src => src.Customer.Name))
                 .ForMember(dest => dest.Status,opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<OrderItem, OrderItemDto>();

            CreateMap<OrderFlow.Domain.Entities.Order, OrderListDto>()
                .ForMember(dest => dest.CustomerName,opt => opt.MapFrom(src => src.Customer.Name))
                .ForMember(dest => dest.Status,opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.ItemCount,opt => opt.MapFrom(src => src.Items.Count));

        }
    }
}
