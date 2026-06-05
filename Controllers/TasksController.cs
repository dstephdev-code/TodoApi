using Microsoft.AspNetCore.Mvc;
using TodoApi.DataAccess.Repositories;
using TodoApi.Model;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController(ITodoTaskRepository todoTaskRepository) : ControllerBase
    {
        //I need to make it so I DI here service, not repository, and all validation and transfering will perform in service, not here!
        // !!!!
        // !!!!
        // !!!!

        private readonly ITodoTaskRepository _todoTaskRepository = todoTaskRepository;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _todoTaskRepository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoTask>> GetById(Guid id)
        {
            var task = await _todoTaskRepository.GetByIdAsync(id);
            if (task == null) return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TodoTask>> Create([FromBody] TodoTaskDTO taskDTO)
        {
            //Validation start

            //Validation end

            var task = new TodoTask
            {
                Id = Guid.NewGuid(),
                Name = taskDTO.Name,
                Description = taskDTO.Description,
                CreatedAt = DateTimeOffset.UtcNow,
                DueDate = taskDTO.DueDate,
                Status = Model.Enums.TaskStatusEnum.Created,
                Priority = taskDTO.Priority
            };

            await _todoTaskRepository.AddAsync(task);
            await _todoTaskRepository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }
    }
}
