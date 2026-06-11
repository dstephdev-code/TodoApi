using TodoApi.Model;

namespace TodoApi.Services
{
    public interface ITodoTasksService
    {
        Task<IEnumerable<TodoTaskDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TodoTaskDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TodoTaskDTO> CreateAsync(TodoTaskCreateDTO todoTaskDTO, CancellationToken cancellationToken = default);
        Task RemoveById(Guid id, CancellationToken cancellationToken = default);
        Task UpdatePartialAsync(Guid id, TodoTaskUpdateDTO todoTaskUpdateDTO, CancellationToken cancellationToken = default);
    }
}