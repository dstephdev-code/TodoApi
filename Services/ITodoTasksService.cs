using TodoApi.Model;

namespace TodoApi.Services
{
    public interface ITodoTasksService
    {
        Task<List<TodoTask>> GetAllAsync();
        Task<TodoTask> GetByIdAsync(Guid id);
        Task<TodoTask> CreateAsync(TodoTaskDTO todoTaskDTO);
        Task RemoveById(Guid id);
    }
}