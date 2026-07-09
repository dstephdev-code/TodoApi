using TodoApp.Api.DataAccess.Repositories;
using TodoApp.Api.Exceptions;
using TodoApp.Api.Model.TaskAssignment.Dto;
using TodoApp.Api.Model.TodoTasks;
using TodoApp.Api.Model.TodoTasks.Dto;
using TodoApp.Api.Model.User;

namespace TodoApp.Api.Services
{
    public class TodoTasksService(ITodoTaskRepository todoTaskRepository, IUserRepository userRepository) : ITodoTasksService
    {
        private readonly ITodoTaskRepository _todoTaskRepository = todoTaskRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<TaskResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var task = await GetOrThrowAsync(id, cancellationToken);

            return MapToDto(task);
        }
        public async Task<IEnumerable<TaskResponse>> SearchAsync(GetTasksQuery query, CancellationToken cancellationToken = default)
        {
            var tasks = await _todoTaskRepository.GetAllAsync(query, cancellationToken);

            return tasks.Select(MapToDto);
        }
        public async Task<TaskResponse> CreateAsync(CreateTaskRequest request, CancellationToken cancellationToken = default)
        {
            var task = new TodoTask(request.Name, request.Description, request.DueDate, request.Priority);

            await _todoTaskRepository.AddAsync(task, cancellationToken);
            await _todoTaskRepository.SaveChangesAsync(cancellationToken);

            return MapToDto(task);
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var task = await GetOrThrowAsync(id, cancellationToken);

            task.EnsureCanBeDeleted();
            _todoTaskRepository.Remove(task);

            await _todoTaskRepository.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(Guid id, PatchTaskRequest request, CancellationToken cancellationToken = default)
        {
            var task = await GetOrThrowAsync(id, cancellationToken);

            task.ChangeName(request.Name);
            task.ChangeDescription(request.Description);
            task.ChangeDueDate(request.DueDate);
            task.ChangePriority(request.Priority);

            await _todoTaskRepository.SaveChangesAsync(cancellationToken);
        }
        public async Task AssignUserAsync(Guid id, AssignUserRequest request, CancellationToken cancellationToken = default)
        {
            var task = await GetOrThrowAsync(id, cancellationToken);
            var user = await GetUserOrThrowAsync(request.UserId, cancellationToken);

            task.AssignUser(user, request.AssignedByUserId, request.Comment);

            await _todoTaskRepository.SaveChangesAsync(cancellationToken);
        }
        public async Task UnassignUserAsync(Guid id, UnassignUserRequest request, CancellationToken cancellationToken = default)
        {
            var task = await GetOrThrowAsync(id, cancellationToken);
            var user = await GetUserOrThrowAsync(request.UserId, cancellationToken);

            task.UnassignUser(user);

            await _todoTaskRepository.SaveChangesAsync(cancellationToken);
        }
        public async Task StartAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var task = await GetOrThrowAsync(id, cancellationToken);

            task.Start();

            await _todoTaskRepository.SaveChangesAsync(cancellationToken);
        }
        public async Task CompleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var task = await GetOrThrowAsync(id, cancellationToken);

            task.Complete();

            await _todoTaskRepository.SaveChangesAsync(cancellationToken);
        }
        public async Task CancelAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var task = await GetOrThrowAsync(id, cancellationToken);

            task.Cancel();

            await _todoTaskRepository.SaveChangesAsync(cancellationToken);
        }

        private async Task<TodoTask> GetOrThrowAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var task = await _todoTaskRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException($"ToDo task by id {id} was not found!");
            return task;
        }
        private async Task<User> GetUserOrThrowAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException($"User by id {id} was not found!");
            return user;
        }
        private static TaskResponse MapToDto(TodoTask task) =>
            new(task.Id, task.Name, task.Description, task.CreatedAt, task.DueDate, task.UpdatedAt, task.Status, task.Priority);
    }
}