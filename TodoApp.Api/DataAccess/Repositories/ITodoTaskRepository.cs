using TodoApp.Api.Model.TodoTasks;
using TodoApp.Api.Model.TodoTasks.Dto;

namespace TodoApp.Api.DataAccess.Repositories
{
    public interface ITodoTaskRepository
    {
        Task<TodoTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<TodoTask>> GetAllAsync(GetTasksQuery query, CancellationToken cancellationToken = default);
        Task AddAsync(TodoTask todoTask, CancellationToken cancellationToken = default);
        void Remove(TodoTask task);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
