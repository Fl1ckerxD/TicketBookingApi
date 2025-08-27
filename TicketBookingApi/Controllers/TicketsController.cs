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

        [HttpGet("user/{userName}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<TicketDto>>> GetByUserId(string userName)
        {
            try
            {
                return await _mediator.Send(new GetTicketsByUserNameQuery(userName));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
    }
}