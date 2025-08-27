using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketBookingApi.Features.Users;
using TicketBookingApi.Features.Users.GetUserByName;
using TicketBookingApi.Features.Users.GetUsers;

namespace TicketBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
            => await _mediator.Send(new GetUsersQuery());

        [HttpGet("{userName}")]
        public async Task<ActionResult<UserDto>> GetUserByName(string userName)
        {
            try
            {
                return await _mediator.Send(new GetUserByNameQuery(userName));
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