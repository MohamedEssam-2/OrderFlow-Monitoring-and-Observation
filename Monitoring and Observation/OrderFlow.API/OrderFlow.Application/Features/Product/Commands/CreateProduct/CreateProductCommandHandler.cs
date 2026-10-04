using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OrderFlow.Application.Abstractions;


namespace OrderFlow.Application.Features.Product.Commands.CreateProduct
{
    public class CreateProductCommandHandler(IApplicationDbContext _context,IMapper _mapper ) : IRequestHandler<CreateProductCommand, int>
    {
        public async Task<int> Handle(CreateProductCommand request,CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Product.Name))
            {
                throw new ArgumentException("Product name is required.");
            }

            if (request.Product.Price <= 0)
            {
                throw new ArgumentException("Product price must be greater than zero.");
            }
            if(request.Product.IsActive == false)
            {
               throw new ArgumentException("Product must be active.");
            }
            var product = _mapper.Map<OrderFlow.Domain.Entities.Product>(request.Product);
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            return product.Id;
        }
    }
}
