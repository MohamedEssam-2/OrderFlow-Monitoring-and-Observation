using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
namespace OrderFlow.Application.Features.Product.Commands.CreateProduct
{
    public sealed record CreateProductCommand(CreateProductDto Product) : IRequest<int>;
    
}
