using Microsoft.EntityFrameworkCore;
using TodoApi.Model;

namespace TodoApi.DataAccess.Repositories
{
    public class TodoTaskRepository(TodoDbContext context) : ITodoTaskRepository
    {
        private readonly TodoDbContext _dbContext = context;

        public async Task<TodoTask?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Tasks.FindAsync(id);
        }

        public async Task<List<TodoTask>> GetAllAsync()
        {
            return await _dbContext.Tasks.ToListAsync();
        }

        public async Task AddAsync(TodoTask todoTask)
        {
            await _dbContext.Tasks.AddAsync(todoTask);
        }

        public void Remove(TodoTask task)
        {
            _dbContext.Tasks.Remove(task);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
