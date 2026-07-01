using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Model.User.Dto;
using TodoApp.Api.Services;

namespace TodoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController (
                IValidator<GetUsersQuery> userGetAllValidator,
                IValidator<CreateUserRequest> userCreateValidator,
                IValidator<UpdateUserRequest> userUpdateValidator,
                IUserService userService
        ) : ControllerBase
    {
        private readonly IValidator<GetUsersQuery> _userGetAllValidator = userGetAllValidator;
        private readonly IValidator<CreateUserRequest> _userCreateValidator = userCreateValidator;
        private readonly IValidator<UpdateUserRequest> _userUpdateValidator = userUpdateValidator;
        private readonly IUserService _userService = userService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll([FromQuery] GetUsersQuery query, CancellationToken cancellationToken)
        {
            var validationResult = await _userGetAllValidator.ValidateAsync(query, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            return Ok(await _userService.GetAllAsync(query, cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> Create([FromBody] CreateUserRequest userDto, CancellationToken cancellationToken)
        {
            var validationResult = await _userCreateValidator.ValidateAsync(userDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var user = await _userService.CreateAsync(userDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveById(Guid id, CancellationToken cancellationToken)
        {
            await _userService.RemoveById(id, cancellationToken);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchById(Guid id, [FromBody] UpdateUserRequest userDto, CancellationToken cancellationToken)
        {
            var validationResult = await _userUpdateValidator.ValidateAsync(userDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await _userService.UpdatePartialAsync(id, userDto, cancellationToken);
            return NoContent();
        }
    }
}
