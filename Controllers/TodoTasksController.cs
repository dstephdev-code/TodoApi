using Microsoft.AspNetCore.Mvc;
using TodoApi.Model;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoTasksController(ITodoTasksService todoTasksService) : ControllerBase
    {
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
            await _todoTasksService.UpdatePartialAsync(id, todoTaskUpdateDTO, cancellationToken);
            return NoContent();
        }
    }
}
