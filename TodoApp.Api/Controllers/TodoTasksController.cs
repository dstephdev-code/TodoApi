using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Model.TaskAssignment.Dto;
using TodoApp.Api.Model.TodoTasks.Dto;
using TodoApp.Api.Services;

namespace TodoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoTasksController(
                IValidator<GetTasksQuery> taskGetAllValidator, 
                IValidator<CreateTaskRequest> taskCreateValidator, 
                IValidator<UpdateTaskRequest> taskUpdateValidator,
                IValidator<AssignUserRequest> taskAssignValidator,
                ITodoTasksService todoTasksService
        ) : ControllerBase
    {
        private readonly IValidator<GetTasksQuery> _taskGetAllValidator = taskGetAllValidator;
        private readonly IValidator<CreateTaskRequest> _taskCreateValidator = taskCreateValidator;
        private readonly IValidator<UpdateTaskRequest> _taskUpdateValidator = taskUpdateValidator;
        private readonly IValidator<AssignUserRequest> _taskAssignValidator = taskAssignValidator;
        private readonly ITodoTasksService _todoTasksService = todoTasksService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponse>>> GetAll([FromQuery] GetTasksQuery query, CancellationToken cancellationToken)
        {
            var validationResult = await _taskGetAllValidator.ValidateAsync(query, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            return Ok(await _todoTasksService.GetAllAsync(query, cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var task = await _todoTasksService.GetByIdAsync(id, cancellationToken);
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponse>> Create([FromBody] CreateTaskRequest taskDto, CancellationToken cancellationToken)
        {
            var validationResult = await _taskCreateValidator.ValidateAsync(taskDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var todoTask = await _todoTasksService.CreateAsync(taskDto, cancellationToken);       
            return CreatedAtAction(nameof(GetById), new { id = todoTask.Id }, todoTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveById(Guid id, CancellationToken cancellationToken)
        {
            await _todoTasksService.RemoveById(id, cancellationToken);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchById(Guid id, [FromBody] UpdateTaskRequest taskDto, CancellationToken cancellationToken)
        {
            var validationResult = await _taskUpdateValidator.ValidateAsync(taskDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await _todoTasksService.UpdatePartialAsync(id, taskDto, cancellationToken);
            return NoContent();
        }

        [HttpPost("{taskId}/assignments")]
        public async Task<IActionResult> AssignUserAsync(Guid taskId, [FromBody] AssignUserRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _taskAssignValidator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await _todoTasksService.AssignUserAsync(taskId, request, cancellationToken);

            return NoContent();
        }
    }
}
