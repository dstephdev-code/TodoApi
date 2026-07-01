using TodoApp.Api.Model.TaskAssignment.Dto;
using TodoApp.Api.Model.TodoTasks.Dto;

namespace TodoApp.Api.Services
{
    public interface ITodoTasksService
    {
        Task<IEnumerable<TaskResponse>> GetAllAsync(GetTasksQuery query, CancellationToken cancellationToken = default);
        Task<TaskResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TaskResponse> CreateAsync(CreateTaskRequest taskDto, CancellationToken cancellationToken = default);
        Task RemoveById(Guid id, CancellationToken cancellationToken = default);
        Task UpdatePartialAsync(Guid id, UpdateTaskRequest taskDto, CancellationToken cancellationToken = default);

        Task AssignUserAsync(Guid id, AssignUserRequest request, CancellationToken cancellationToken = default);
    }
}