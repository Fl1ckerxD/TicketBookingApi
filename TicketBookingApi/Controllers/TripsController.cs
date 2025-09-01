using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketBookingApi.Features.Trips;
using TicketBookingApi.Features.Trips.CreateTrip;
using TicketBookingApi.Features.Trips.DeleteTrip;
using TicketBookingApi.Features.Trips.GetTripById;
using TicketBookingApi.Features.Trips.GetTrips;
using TicketBookingApi.Features.Trips.UpdateTrip;

namespace TicketBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TripsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TripsController> _logger;

        public TripsController(IMediator mediator, ILogger<TripsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [EndpointSummary("Получить список поездок с возможностью фильтрации по параметрам")]
        [EndpointDescription("Возвращает список всех поездок. Можно фильтровать по пункту отправления, пункту назначения и дате")]
        [ProducesResponseType(typeof(List<TripDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TripDto>>> Get(string? from, string? to, DateTime? date)
        {
            try
            {
                return await _mediator.Send(new GetTripsQuery(from, to, date));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [EndpointSummary("Получить детали поездки по идентификатору")]
        [EndpointDescription("Возвращает детали конкретной поездки по её уникальному идентификатору")]
        [ProducesResponseType(typeof(TripDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TripDto>> GetById(int id)
        {
            try
            {
                return await _mediator.Send(new GetTripQuery(id));
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [EndpointSummary("Создать новую поездку")]
        [EndpointDescription("Позволяет администраторам создавать новые поездки, указывая пункт отправления, пункт назначения, время отправления и прибытия, количество мест и цену")]
        [ProducesResponseType(typeof(TripDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<TripDto>> Create([FromBody] CreateTripCommand command)
        {
            try
            {
                var tripDto = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = tripDto.Id }, tripDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [EndpointSummary("Обновить информацию о поездке")]
        [EndpointDescription("Позволяет администраторам обновлять детали существующих поездок, включая пункт отправления, пункт назначения, время отправления и прибытия, количество мест и цену")]
        [ProducesResponseType(typeof(TripDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<TripDto>> Update(int id, [FromBody] UpdateTripCommand command)
        {
            if (id != command.Id)
                return BadRequest("Идентификатор в URL не совпадает с идентификатором в теле запроса");
            try
            {
                var tripDto = await _mediator.Send(command);
                return Ok(tripDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [EndpointSummary("Удалить поездку")]
        [EndpointDescription("Позволяет администраторам удалять существующие поездки по их уникальному идентификатору")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _mediator.Send(new DeleteTripCommand(id));
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