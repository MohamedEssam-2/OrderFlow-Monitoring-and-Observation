using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Features.Customer.Commands.CreateCustomer;
using OrderFlow.Application.Features.Customer.Queries.GetAllCustomer;

namespace OrderFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDTO customerDto, CancellationToken cancellationToken)
        {
            var command = new CreateCustomerCommand(customerDto);
            var customerId = await _mediator.Send(command,cancellationToken);
            return Ok(new { id = customerId });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers(CancellationToken cancellationToken)
        {
            var customers = await _mediator.Send(new GetCustomerQuery(), cancellationToken);
            return Ok(customers);
        }

    }
}
