using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Model.TaskAssignment.Dto;
using TodoApp.Api.Model.TodoTasks.Dto;
using TodoApp.Api.Services;

namespace TodoApp.Api.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TodoTasksController(
                IValidator<GetTasksQuery> taskGetAllValidator, 
                IValidator<CreateTaskRequest> taskCreateValidator, 
                IValidator<PatchTaskRequest> taskUpdateValidator,
                IValidator<AssignUserRequest> assignUserValidator,
                IValidator<UnassignUserRequest> unassignUserValidator,
                ITodoTasksService todoTasksService
        ) : ControllerBase
    {
        private readonly IValidator<GetTasksQuery> _taskGetAllValidator = taskGetAllValidator;
        private readonly IValidator<CreateTaskRequest> _taskCreateValidator = taskCreateValidator;
        private readonly IValidator<PatchTaskRequest> _taskUpdateValidator = taskUpdateValidator;
        private readonly IValidator<AssignUserRequest> _assignUserValidator = assignUserValidator;
        private readonly IValidator<UnassignUserRequest> _unassignUserValidator = unassignUserValidator;
        private readonly ITodoTasksService _todoTasksService = todoTasksService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponse>>> GetAll([FromQuery] GetTasksQuery query, CancellationToken cancellationToken)
        {
            var validationResult = await _taskGetAllValidator.ValidateAsync(query, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            return Ok(await _todoTasksService.SearchAsync(query, cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponse>> Get(Guid id, CancellationToken cancellationToken)
        {
            var task = await _todoTasksService.GetByIdAsync(id, cancellationToken);
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponse>> Create([FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _taskCreateValidator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var todoTask = await _todoTasksService.CreateAsync(request, cancellationToken);       
            return CreatedAtAction(nameof(Get), new { id = todoTask.Id }, todoTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _todoTasksService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] PatchTaskRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _taskUpdateValidator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await _todoTasksService.UpdateAsync(id, request, cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/assignments")]
        public async Task<IActionResult> AssignUser(Guid id, [FromBody] AssignUserRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _assignUserValidator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await _todoTasksService.AssignUserAsync(id, request, cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/unassign")]
        public async Task<IActionResult> UnassignUser(Guid id, [FromBody] UnassignUserRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _unassignUserValidator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await _todoTasksService.UnassignUserAsync(id, request, cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/start")]
        public async Task<IActionResult> Start(Guid id, CancellationToken cancellationToken)
        {
            await _todoTasksService.StartAsync(id, cancellationToken);
            return NoContent();
        }
        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(Guid id, CancellationToken cancellationToken)
        {
            await _todoTasksService.CompleteAsync(id, cancellationToken);
            return NoContent();
        }
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
        {
            await _todoTasksService.CancelAsync(id, cancellationToken);
            return NoContent();
        }

    }
}