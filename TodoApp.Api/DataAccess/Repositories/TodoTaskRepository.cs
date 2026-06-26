using Microsoft.EntityFrameworkCore;
using TodoApi.Model;

namespace TodoApi.DataAccess.Repositories
{
    public class TodoTaskRepository(TodoDbContext context) : ITodoTaskRepository
    {
        private readonly TodoDbContext _dbContext = context;

        public async Task<TodoTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<TodoTask>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Tasks.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(TodoTask todoTask, CancellationToken cancellationToken = default)
        {
            await _dbContext.Tasks.AddAsync(todoTask, cancellationToken);
        }

        public void Remove(TodoTask task)
        {
            _dbContext.Tasks.Remove(task);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
