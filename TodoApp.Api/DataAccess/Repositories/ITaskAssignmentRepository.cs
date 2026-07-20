using TodoApp.Api.Model.TaskAssignment;
using TodoApp.Api.Model.TaskAssignment.Dto;

namespace TodoApp.Api.DataAccess.Repositories
{
    public interface ITaskAssignmentRepository
    {
        Task<List<TaskAssignment>> GetAllAsync(TaskAssignmentQuery query, CancellationToken cancellationToken = default);
        Task<TaskAssignment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}