using TodoApp.Api.DataAccess.Repositories;
using TodoApp.Api.Exceptions;
using TodoApp.Api.Model.TaskAssignment.Dto;
using TodoApp.Api.Model.TodoTasks;
using TodoApp.Api.Model.TodoTasks.Dto;
using TodoApp.Api.Model.User;

namespace TodoApp.Api.Services
{
    public class TodoTasksService(ITodoTaskRepository todoTaskRepository, IUserRepository userRepository, ILogger<TodoTasksService> logger) : ITodoTasksService
    {
        private readonly ITodoTaskRepository _todoTaskRepository = todoTaskRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ILogger<TodoTasksService> _logger = logger;

        public async Task<TaskResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var todoTask = await GetOrThrowAsync(id, cancellationToken);

            return MapToDto(todoTask);
        }
        public async Task<IEnumerable<TaskResponse>> GetAllAsync(GetTasksQuery query, CancellationToken cancellationToken = default)
        {
            var todoTasks = await _todoTaskRepository.GetAllAsync(query, cancellationToken);

            return todoTasks.Select(MapToDto);
        }
        public async Task<TaskResponse> CreateAsync(CreateTaskRequest taskDto, CancellationToken cancellationToken = default)
        {
            var todoTask = new TodoTask(taskDto.Name, taskDto.Description, taskDto.DueDate, taskDto.Priority);

            await _todoTaskRepository.AddAsync(todoTask, cancellationToken);
            await _todoTaskRepository.SaveChangesAsync(cancellationToken);

            return MapToDto(todoTask);
        }

        public async Task RemoveById(Guid id, CancellationToken cancellationToken = default)
        {
            var todoTask = await GetOrThrowAsync(id, cancellationToken);
            // I'm not deleting it right away because we need first to check if we have connected users.
            // For now there is no check for it, I simply checking null, but its temporary
            _todoTaskRepository.Remove(todoTask);

            await _todoTaskRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdatePartialAsync(Guid id, UpdateTaskRequest taskDto, CancellationToken cancellationToken = default)
        {
            var todoTask = await GetOrThrowAsync(id, cancellationToken);
            todoTask.ChangeName(taskDto.Name);
            todoTask.ChangeDescription(taskDto.Description);
            todoTask.ChangeDueDate(taskDto.DueDate);
            // todoTask.ChangeStatus(taskDto.Status); //delete, added entity methods
            todoTask.ChangePriority(taskDto.Priority);

            await _todoTaskRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task AssignUserAsync(Guid id, AssignUserRequest request, CancellationToken cancellationToken = default)
        {
            TodoTask task = await GetOrThrowAsync(id, cancellationToken);
            User user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException($"User with id {request.UserId} was not found!");

            task.AssignUser(user, request.AssignedByUserId, request.Comment);
            await _todoTaskRepository.SaveChangesAsync(cancellationToken);
        }

        private async Task<TodoTask> GetOrThrowAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var todoTask = await _todoTaskRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException($"ToDo task by id {id} was not found!");
            return todoTask;
        }
        private static TaskResponse MapToDto(TodoTask task) =>
            new(task.Id, task.Name, task.Description, task.CreatedAt, task.DueDate, task.UpdatedAt, task.Status, task.Priority);
    }
}
