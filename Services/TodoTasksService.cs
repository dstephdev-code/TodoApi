using TodoApi.DataAccess.Repositories;
using TodoApi.Exceptions;
using TodoApi.Model;

namespace TodoApi.Services
{
    /***
     * Here we have CA1873. To solve this we need to make a Source Generators for logging, but for now it looks for me as overengineering.
     * I probably will do something here in future, but for time-being I'll use pragma. So make sure there are no expensive "doings" in methods using strings!!!
     ***/

#pragma warning disable CA1873
    public class TodoTasksService(ITodoTaskRepository todoTaskRepository, ILogger<TodoTasksService> logger) : ITodoTasksService
    {
        private readonly ITodoTaskRepository _todoTaskRepository = todoTaskRepository;
        private readonly ILogger<TodoTasksService> _logger = logger;

        public async Task<TodoTaskDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // I don't see any way we should do anything if there is no task by id except for highlighting it. Even in scaled app. So just basic throw if null.
            var todoTask = await GetOrThrowAsync(id, cancellationToken);

            return MapToDto(todoTask);
        }
        public async Task<IEnumerable<TodoTaskDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            // Do I need any logic in getting list? Maybe sorting in future. For now we even don't have any possible exception here.
            _logger.LogInformation("Trying to get all the tasks");
            var todoTasks = await _todoTaskRepository.GetAllAsync(cancellationToken);
            _logger.LogInformation("Succesfully got all the tasks");

            return todoTasks.Select(MapToDto);
        }
        public async Task<TodoTaskDTO> CreateAsync(TodoTaskCreateDTO todoTaskDTO, CancellationToken cancellationToken = default)
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
            await _todoTaskRepository.AddAsync(todoTask, cancellationToken);

            await _todoTaskRepository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Succesfully created task with id {TaskId}", todoTask.Id);

            return MapToDto(todoTask);
        }

        public async Task RemoveById(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Trying to remove a task");
            var todoTask = await GetOrThrowAsync(id, cancellationToken);
            // I'm not deleting it right away because we need first to check if we have connected users.
            // For now there is no check for it, I simply checking null, but its temporary
            _todoTaskRepository.Remove(todoTask);

            await _todoTaskRepository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Succesfully removed task with id {TaskId}", id);
        }

        public async Task UpdatePartialAsync(Guid id, TodoTaskUpdateDTO todoTaskUpdateDTO, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Trying to update a task");
            var todoTask = await GetOrThrowAsync(id, cancellationToken);
            //Validation of DTO here!

            //----------------------

            todoTask.Name = todoTaskUpdateDTO.Name ?? todoTask.Name;
            todoTask.Description = todoTaskUpdateDTO.Description ?? todoTask.Description;
            todoTask.DueDate = todoTaskUpdateDTO.DueDate ?? todoTask.DueDate;
            todoTask.Status = todoTaskUpdateDTO.Status != null ? todoTaskUpdateDTO.Status.Value : todoTask.Status;
            todoTask.Priority = todoTaskUpdateDTO.Priority != null ? todoTaskUpdateDTO.Priority.Value : todoTask.Priority;

            todoTask.UpdatedAt = DateTimeOffset.UtcNow;

            await _todoTaskRepository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Succesfully updated task with id {TaskId}", id);
        }

        private async Task<TodoTask> GetOrThrowAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Trying to get task with id {TaskId}", id);
            var todoTask = await _todoTaskRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException($"ToDo task by id {id} was not found!");
            _logger.LogInformation("Succesfully got task with id {TaskId}", id);
            return todoTask;
        }
        private static TodoTaskDTO MapToDto(TodoTask task) => new()
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            CreatedAt = task.CreatedAt,
            DueDate = task.DueDate,
            Status = task.Status,
            Priority = task.Priority
        };
    }
#pragma warning restore CA1873
}
