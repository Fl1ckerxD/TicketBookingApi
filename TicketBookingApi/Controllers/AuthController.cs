using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketBookingApi.Features.Auth;
using TicketBookingApi.Features.Auth.ExternalLogin;
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

        [HttpGet("signin/{provider}")]
        public IActionResult SignIn(string provider, string returnUrl = "/")
        {
            var redirectUrl = Url.Action(nameof(Callback), "Auth", new { returnUrl, provider }, Request.Scheme);
            var props = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(props, provider);
        }

        [HttpGet("callback")]
        public async Task<ActionResult<AuthResponseDto>> Callback(string returnUrl = "/", string provider = "Google")
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

            if (!result.Succeeded)
                return Unauthorized(new { error = "External login failed" });

            var claims = result.Principal.Identities.First().Claims.ToList();

            return await _mediator.Send(new ExternalLoginCommand(claims, provider));
        }
    }
}