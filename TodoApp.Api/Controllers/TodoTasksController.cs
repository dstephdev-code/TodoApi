using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Model;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoTasksController(IValidator<TodoTaskCreateDTO> taskCreateValidator, IValidator<TodoTaskUpdateDTO> taskUpdateValidator, ITodoTasksService todoTasksService) : ControllerBase
    {
        private readonly IValidator<TodoTaskCreateDTO> _taskCreateValidator = taskCreateValidator;
        private readonly IValidator<TodoTaskUpdateDTO> _taskUpdateValidator = taskUpdateValidator;
        private readonly ITodoTasksService _todoTasksService = todoTasksService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoTaskDTO>>> GetAll(CancellationToken cancellationToken) 
            => Ok(await _todoTasksService.GetAllAsync(cancellationToken)); // Maybe show NoContent() when list is empty?

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoTaskDTO>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var task = await _todoTasksService.GetByIdAsync(id, cancellationToken);
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TodoTaskDTO>> Create([FromBody] TodoTaskCreateDTO taskDTO, CancellationToken cancellationToken)
        {
            var validationResult = await _taskCreateValidator.ValidateAsync(taskDTO, cancellationToken);

            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            var todoTask = await _todoTasksService.CreateAsync(taskDTO, cancellationToken);       
            return CreatedAtAction(nameof(GetById), new { id = todoTask.Id }, todoTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveById(Guid id, CancellationToken cancellationToken)
        {
            await _todoTasksService.RemoveById(id, cancellationToken);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchById(Guid id, [FromBody] TodoTaskUpdateDTO todoTaskUpdateDTO, CancellationToken cancellationToken)
        {
            var validationResult = await _taskUpdateValidator.ValidateAsync(todoTaskUpdateDTO, cancellationToken);

            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            await _todoTasksService.UpdatePartialAsync(id, todoTaskUpdateDTO, cancellationToken);
            return NoContent();
        }
    }
}
