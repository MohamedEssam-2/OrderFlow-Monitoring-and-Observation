using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Features.Order.Commands.CreateOrder;
using OrderFlow.Application.Features.Order.Queries;
using OrderFlow.Application.Features.Order.Queries.GetOrderByid;
using OrderFlow.Application.Features.Order.Queries.GetOrders;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO orderDto, CancellationToken cancellationToken)
        {
            var command = new CreateOrderCommand(orderDto);
            var orderId = await _mediator.Send(command, cancellationToken);
            return Ok(new { id = orderId });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id, CancellationToken cancellationToken)
        {
            var query = new GetOrderByIdQuery(id);
            var orderDetails = await _mediator.Send(query, cancellationToken);
            return Ok(orderDetails);
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders(CancellationToken cancellationToken)
        {
            var query = new GetOrdersQuery();
            var orders = await _mediator.Send(query, cancellationToken);
            return Ok(orders);
        }
    }
}
