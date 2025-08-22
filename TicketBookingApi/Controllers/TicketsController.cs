using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketBookingApi.Features.Tickets;

namespace TicketBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Buy([FromBody] BuyTicketCommand command)
            => Ok(await _mediator.Send(command));
    }
}