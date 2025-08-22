using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketBookingApi.Features.Trips;

namespace TicketBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TripsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? from, [FromQuery] string? to, [FromQuery] DateTime? date)
        {
            var result = await _mediator.Send(new GetTripsQuery(from, to, date));
            return Ok(result);
        }
    }
}