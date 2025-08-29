using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketBookingApi.Features.Auth;
using TicketBookingApi.Features.Auth.Login;
using TicketBookingApi.Features.Auth.RefreshToken;
using TicketBookingApi.Features.Auth.Register;

namespace TicketBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginCommand command)
        {
            try
            {
                var authResponse = await _mediator.Send(command);
                return authResponse;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            try
            {
                var authResponse = await _mediator.Send(command);
                return authResponse;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}