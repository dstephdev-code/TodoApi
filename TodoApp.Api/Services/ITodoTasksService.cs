using TodoApp.Api.Model.TaskAssignment.Dto;
using TodoApp.Api.Model.TodoTasks.Dto;

namespace TodoApp.Api.Services
{
    public interface ITodoTasksService
    {
        Task<IEnumerable<TaskResponse>> SearchAsync(GetTasksQuery query, CancellationToken cancellationToken = default);
        Task<TaskResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TaskResponse> CreateAsync(CreateTaskRequest request, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Guid id, PatchTaskRequest request, CancellationToken cancellationToken = default);
        Task AssignUserAsync(Guid id, AssignUserRequest request, CancellationToken cancellationToken = default);
        Task UnassignUserAsync(Guid id, UnassignUserRequest request, CancellationToken cancellationToken = default);
        Task StartAsync(Guid id, CancellationToken cancellationToken = default);
        Task CompleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task CancelAsync(Guid id, CancellationToken cancellationToken = default);
    }
}