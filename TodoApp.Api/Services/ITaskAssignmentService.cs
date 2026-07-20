using TodoApp.Api.Model.TaskAssignment.Dto;

namespace TodoApp.Api.Services
{
    public interface ITaskAssignmentService
    {
        Task<TaskAssignmentResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TaskAssignmentResponse>> SearchAsync(TaskAssignmentQuery query, CancellationToken cancellationToken = default);
    }
}