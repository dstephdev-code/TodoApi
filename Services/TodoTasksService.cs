using TodoApi.DataAccess.Repositories;
using TodoApi.Model;

namespace TodoApi.Services
{
    public class TodoTasksService(ITodoTaskRepository todoTaskRepository) : ITodoTasksService
    {
        private readonly ITodoTaskRepository _todoTaskRepository = todoTaskRepository;

        public async Task<TodoTask> GetByIdAsync(Guid id)
        {
            // I don't see any way we should do anything if there is no task by id except for highlighting it. Even in scaled app. So just basic throw if null.
            var todoTask = await _todoTaskRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"ToDo task by id {id} is not found!");

            return todoTask;
        }
        public async Task<List<TodoTask>> GetAllAsync()
        {
            // Do I need any logic in getting list? Maybe sorting in future. For now we even don't have any possible exception here.
            return await _todoTaskRepository.GetAllAsync();
        }
        public async Task<TodoTask> CreateAsync(TodoTaskDTO todoTaskDTO)
        {
            var todoTask = new TodoTask
            {
                Id = Guid.NewGuid(),
                Name = todoTaskDTO.Name,
                Description = todoTaskDTO.Description,
                CreatedAt = DateTimeOffset.UtcNow,
                DueDate = todoTaskDTO.DueDate,
                Status = Model.Enums.TaskStatusEnum.Created,
                Priority = todoTaskDTO.Priority
            };

            await _todoTaskRepository.AddAsync(todoTask);
            await _todoTaskRepository.SaveChangesAsync();

            return todoTask;
        }

        public async Task RemoveById(Guid id)
        {
            // I'm not deleting it right away because we need first to check if we have connected users.
            // For now there is no check for it, I simply checking null, but its temporary
            var todoTask = await _todoTaskRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"ToDo task by id {id} is not found!");

            _todoTaskRepository.Remove(todoTask);
            await _todoTaskRepository.SaveChangesAsync();
        }

        public async Task UpdatePartialAsync(Guid id, TodoTaskUpdateDTO todoTaskUpdateDTO)
        {
            var todoTask = await _todoTaskRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"ToDo task by id {id} is not found!");

            //Validation of DTO here!

            //----------------------

            todoTask.Name = todoTaskUpdateDTO.Name ?? todoTask.Name;
            todoTask.Description = todoTaskUpdateDTO.Description ?? todoTask.Description;
            todoTask.DueDate = todoTaskUpdateDTO.DueDate ?? todoTask.DueDate;
            todoTask.Status = todoTaskUpdateDTO.Status != null ? todoTaskUpdateDTO.Status.Value : todoTask.Status;
            todoTask.Priority = todoTaskUpdateDTO.Priority != null ? todoTaskUpdateDTO.Priority.Value : todoTask.Priority;

            todoTask.UpdatedAt = DateTimeOffset.UtcNow;

            await _todoTaskRepository.SaveChangesAsync();

        }
    }
}
