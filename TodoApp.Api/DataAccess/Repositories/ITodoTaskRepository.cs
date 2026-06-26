using TodoApp.Api.Model;

namespace TodoApp.Api.DataAccess.Repositories
{
    public interface ITodoTaskRepository
    {
        Task<TodoTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<TodoTask>> GetAllAsync(TodoTaskSearchQuery query, CancellationToken cancellationToken = default);
        Task AddAsync(TodoTask todoTask, CancellationToken cancellationToken = default);
        void Remove(TodoTask task);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
