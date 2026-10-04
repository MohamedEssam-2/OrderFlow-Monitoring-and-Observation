using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Features.Dashboard.Queries.GetOrderDashboard;

namespace OrderFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController(IMediator _mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetOrderDashboard(CancellationToken cancellationToken)
        {
            var query = new GetOrderDashboardQuery();
            var dashboardData = await _mediator.Send(query, cancellationToken);
            return Ok(dashboardData);
        }
    }
}
