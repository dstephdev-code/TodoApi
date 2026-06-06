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
        public async Task<ActionResult<TodoTask>> GetAll() => Ok(await _todoTasksService.GetAllAsync()); // Maybe show NoContent() when list is empty?

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoTask>> GetById(Guid id)
        {
            var task = await _todoTasksService.GetByIdAsync(id);
            // There will be middleware to catch problems, so no try {} catch {} for now.
            if (task == null) return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TodoTask>> Create([FromBody] TodoTaskDTO taskDTO)
        {
            var todoTask = await _todoTasksService.CreateAsync(taskDTO);

            if (todoTask == null) return Problem(detail: "Couldn't create task", title: "Internal server error");
            
            return CreatedAtAction(nameof(GetById), new { id = todoTask.Id }, todoTask);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoTask>> RemoveById(Guid id)
        {
            await _todoTasksService.RemoveById(id);
            // Check on exceptions when I'll add middleware
            return NoContent();
        }
    }
}
