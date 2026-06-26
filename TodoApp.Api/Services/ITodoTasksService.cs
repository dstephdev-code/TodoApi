using TodoApp.Api.Model;

namespace TodoApp.Api.Services
{
    public interface ITodoTasksService
    {
        Task<IEnumerable<TodoTaskDTO>> GetAllAsync(TodoTaskSearchQuery query, CancellationToken cancellationToken = default);
        Task<TodoTaskDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TodoTaskDTO> CreateAsync(TodoTaskCreateDTO todoTaskDTO, CancellationToken cancellationToken = default);
        Task RemoveById(Guid id, CancellationToken cancellationToken = default);
        Task UpdatePartialAsync(Guid id, TodoTaskUpdateDTO todoTaskUpdateDTO, CancellationToken cancellationToken = default);
    }
}