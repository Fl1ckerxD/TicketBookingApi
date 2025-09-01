using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketBookingApi.Features.Tickets;
using TicketBookingApi.Features.Tickets.BuyTicket;
using TicketBookingApi.Features.Tickets.CancelTicket;
using TicketBookingApi.Features.Tickets.GetMyTickets;
using TicketBookingApi.Features.Tickets.GetTicketsByUser;

namespace TicketBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(IMediator mediator, ILogger<TicketsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [EndpointSummary("Покупка билета")]
        [EndpointDescription("Позволяет авторизованному пользователю купить билет на указанный рейс и место")]
        [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TicketDto>> Buy([FromBody] BuyTicketCommand command)
        {
            try
            {
                var ticket = await _mediator.Send(command);
                return CreatedAtAction(nameof(Buy), new { id = ticket.Id }, ticket);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userName}")]
        [Authorize(Roles = "Admin")]
        [EndpointSummary("Получение билетов пользователя по имени")]
        [EndpointDescription("Позволяет администратору получить все билеты, купленные указанным пользователем")]
        [ProducesResponseType(typeof(List<TicketDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<TicketDto>>> GetByUserName(string userName)
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
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet("me")]
        [EndpointSummary("Получение моих билетов")]
        [EndpointDescription("Позволяет авторизованному пользователю получить все свои купленные билеты")]
        [ProducesResponseType(typeof(IEnumerable<TicketDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetMyTickets()
        {
            try
            {
                var tickets = await _mediator.Send(new GetMyTicketsQuery());
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpDelete("{id:guid}")]
        [EndpointSummary("Отмена билета")]
        [EndpointDescription("Позволяет авторизованному пользователю отменить купленный билет по его идентификатору")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(Guid id)
        {
            try
            {
                await _mediator.Send(new CancelTicketCommand(id));
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
    }
}