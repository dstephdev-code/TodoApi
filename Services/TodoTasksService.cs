using TodoApi.DataAccess.Repositories;
using TodoApi.Model;

namespace TodoApi.Services
{
    public class TodoTasksService(ITodoTaskRepository todoTaskRepository, ILogger<TodoTasksService> logger) : ITodoTasksService
    {
        private readonly ITodoTaskRepository _todoTaskRepository = todoTaskRepository;
        private readonly ILogger<TodoTasksService> _logger = logger;

        public async Task<TodoTask> GetByIdAsync(Guid id)
        {
            // I don't see any way we should do anything if there is no task by id except for highlighting it. Even in scaled app. So just basic throw if null.
            _logger.LogInformation("Trying to get task with id {TaskId}", id);
            var todoTask = await _todoTaskRepository.GetByIdAsync(id);

            // In future we need somehow make this check easier so i will DRY
            if (todoTask == null)
            {
                _logger.LogWarning("ToDo task by id {TaskId} was not found!", id);
                throw new KeyNotFoundException($"ToDo task by id {id} was not found!");
            }

            _logger.LogInformation("Succesfully got task with id {TaskId}", id);
            return todoTask;
        }
        public async Task<List<TodoTask>> GetAllAsync()
        {
            // Do I need any logic in getting list? Maybe sorting in future. For now we even don't have any possible exception here.
            _logger.LogInformation("Trying to get all the tasks");
            return await _todoTaskRepository.GetAllAsync();
        }
        public async Task<TodoTask> CreateAsync(TodoTaskDTO todoTaskDTO)
        {
            _logger.LogInformation("Trying to create new task");
            var todoTask = new TodoTask
            {
                Id = Guid.NewGuid(),
                Name = todoTaskDTO.Name,
                Description = todoTaskDTO.Description,
                CreatedAt = DateTimeOffset.UtcNow,
                DueDate = todoTaskDTO.DueDate,
                Status = Model.Enums.TaskStatusEnum.Created,
                Priority = todoTaskDTO.Priority
            };

            // Validation and bd is working check in future
            await _todoTaskRepository.AddAsync(todoTask);
            await _todoTaskRepository.SaveChangesAsync();

            _logger.LogInformation("Succesfully created task with id {TaskId}", todoTask.Id);

            return todoTask;
        }

        public async Task RemoveById(Guid id)
        {
            _logger.LogInformation("Trying to get task with id {TaskId} for deletion", id);
            // I'm not deleting it right away because we need first to check if we have connected users.
            // For now there is no check for it, I simply checking null, but its temporary
            var todoTask = await _todoTaskRepository.GetByIdAsync(id);
            if (todoTask == null)
            {
                _logger.LogWarning("ToDo task by id {TaskId} was not found!", id);
                throw new KeyNotFoundException($"ToDo task by id {id} was not found!");
            }

            _todoTaskRepository.Remove(todoTask);
            await _todoTaskRepository.SaveChangesAsync();

            _logger.LogInformation("Succesfully removed task with id {TaskId}", id);
        }

        public async Task UpdatePartialAsync(Guid id, TodoTaskUpdateDTO todoTaskUpdateDTO)
        {
            _logger.LogInformation("Trying to get task with id {TaskId} for updating", id);
            var todoTask = await _todoTaskRepository.GetByIdAsync(id);
            if (todoTask == null)
            {
                throw new KeyNotFoundException($"ToDo task by id {id} was not found!");
            }

            //Validation of DTO here!

            //----------------------

            todoTask.Name = todoTaskUpdateDTO.Name ?? todoTask.Name;
            todoTask.Description = todoTaskUpdateDTO.Description ?? todoTask.Description;
            todoTask.DueDate = todoTaskUpdateDTO.DueDate ?? todoTask.DueDate;
            todoTask.Status = todoTaskUpdateDTO.Status != null ? todoTaskUpdateDTO.Status.Value : todoTask.Status;
            todoTask.Priority = todoTaskUpdateDTO.Priority != null ? todoTaskUpdateDTO.Priority.Value : todoTask.Priority;

            todoTask.UpdatedAt = DateTimeOffset.UtcNow;

            await _todoTaskRepository.SaveChangesAsync();
            _logger.LogInformation("Succesfully updated task with id {TaskId}", id);
        }
    }
}
