using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketBookingApi.Features.Users;
using TicketBookingApi.Features.Users.DeleteUser;
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
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [EndpointSummary("Получение списка всех пользователей")]
        [EndpointDescription("Возвращает список всех пользователей системы")]
        [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            try
            {
                return await _mediator.Send(new GetUsersQuery());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet("{userName}")]
        [EndpointSummary("Получение пользователя по имени")]
        [EndpointDescription("Возвращает данные пользователя по его имени")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpDelete("{userName}")]
        [EndpointSummary("Удаление пользователя по имени")]
        [EndpointDescription("Удаляет пользователя из системы по его имени")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            try
            {
                await _mediator.Send(new DeleteUserCommand(userName));
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