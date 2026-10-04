using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OrderFlow.Application.Features.Product.Commands.CreateProduct;
using OrderFlow.Application.Features.Product.Queries.GetAllProducts;


namespace OrderFlow.Application.Features.Product
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductDto, Domain.Entities.Product>();
            CreateMap<ProductDto, Domain.Entities.Product>().ReverseMap();

        }
    }
}
