using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketBookingApi.Features.Trips;
using TicketBookingApi.Features.Trips.CreateTrip;
using TicketBookingApi.Features.Trips.GetTripById;
using TicketBookingApi.Features.Trips.GetTrips;

namespace TicketBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TripsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TripsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<List<TripDto>>> Get(string? from, string? to, DateTime? date)
            => await _mediator.Send(new GetTripsQuery(from, to, date));

        [HttpGet("{id}")]
        public async Task<ActionResult<TripDto>> GetById(int id)
            => await _mediator.Send(new GetTripQuery(id));

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TripDto>> Create([FromBody] CreateTripCommand command)
        {
            var tripDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = tripDto.Id }, tripDto);
        }
    }
}