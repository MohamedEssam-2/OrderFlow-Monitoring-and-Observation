using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OrderFlow.Application.Features.Customer.Commands.CreateCustomer;
using OrderFlow.Application.Features.Customer.Queries.GetAllCustomer;

namespace OrderFlow.Application.Features.Customer
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CreateCustomerDTO, Domain.Entities.Customer>().ReverseMap();
            CreateMap<CustomerDTO, Domain.Entities.Customer>().ReverseMap();

        }
    }
}
