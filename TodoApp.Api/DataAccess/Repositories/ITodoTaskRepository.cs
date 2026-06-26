using TodoApi.Model;

namespace TodoApi.DataAccess.Repositories
{
    public interface ITodoTaskRepository
    {
        Task<TodoTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<TodoTask>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(TodoTask todoTask, CancellationToken cancellationToken = default);
        void Remove(TodoTask task);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
