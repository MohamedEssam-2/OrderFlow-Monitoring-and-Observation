using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Features.Product.Commands.CreateProduct;
using OrderFlow.Application.Features.Product.Queries.GetAllProducts;

namespace OrderFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto dto, CancellationToken cancellationToken)
        {
            var productId = await _mediator.Send(new CreateProductCommand(dto), cancellationToken);
            return Ok(new
            {
                id = productId
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
        {
            var products = await _mediator.Send(new GetProductsQuery(), cancellationToken);
            return Ok(products);
        }
    }
}
