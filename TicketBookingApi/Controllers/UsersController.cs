using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketBookingApi.Features.Users;
using TicketBookingApi.Features.Users.GetUsers;

namespace TicketBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize (Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return users;
        }
    }
}