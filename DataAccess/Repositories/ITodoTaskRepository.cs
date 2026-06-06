using TodoApi.Model;

namespace TodoApi.DataAccess.Repositories
{
    public interface ITodoTaskRepository
    {
        Task<TodoTask?> GetByIdAsync(Guid id);
        Task<List<TodoTask>> GetAllAsync();
        Task AddAsync(TodoTask todoTask);
        void Remove(TodoTask task);
        Task SaveChangesAsync();

    }
}
