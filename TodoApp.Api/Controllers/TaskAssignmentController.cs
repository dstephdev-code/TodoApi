using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Model.TaskAssignment.Dto;
using TodoApp.Api.Services;

namespace TodoApp.Api.Controllers
{
    [ApiController]
    [Route("api/task_assignments")]
    public class TaskAssignmentController(
            IValidator<TaskAssignmentQuery> getAllValidator,
            ITaskAssignmentService taskAssignmentService
        ) : ControllerBase
    {
        private readonly IValidator<TaskAssignmentQuery> _getAllValidator = getAllValidator;
        private readonly ITaskAssignmentService _taskAssignmentService = taskAssignmentService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskAssignmentResponse>>> GetAll([FromQuery] TaskAssignmentQuery query, CancellationToken cancellationToken)
        {
            var validationResult = await _getAllValidator.ValidateAsync(query, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            return Ok(await _taskAssignmentService.SearchAsync(query, cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskAssignmentResponse>> Get(Guid id, CancellationToken cancellationToken)
        {
            var taskAssignment = await _taskAssignmentService.GetByIdAsync(id, cancellationToken);
            return Ok(taskAssignment);
        }
    }
}
