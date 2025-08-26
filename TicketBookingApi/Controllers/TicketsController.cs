using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketBookingApi.Features.Tickets;
using TicketBookingApi.Features.Tickets.BuyTicket;
using TicketBookingApi.Features.Tickets.GetTicketsByUser;

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

        [HttpGet("user/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<TicketDto>>> GetByUserId(Guid id)
            => await _mediator.Send(new GetTicketsByUserIdQuery(id));
    }
}