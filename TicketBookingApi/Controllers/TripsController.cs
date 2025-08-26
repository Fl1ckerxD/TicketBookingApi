using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketBookingApi.Features.Trips;

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
        public async Task<ActionResult<TripDto>> Get(int id)
            => await _mediator.Send(new GetTripQuery(id));

    }
}