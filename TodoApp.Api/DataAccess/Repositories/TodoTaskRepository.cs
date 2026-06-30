using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Model;

namespace TodoApp.Api.DataAccess.Repositories
{
    public class TodoTaskRepository(TodoDbContext context) : ITodoTaskRepository
    {
        private readonly TodoDbContext _dbContext = context;

        public async Task<TodoTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Tasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<TodoTask>> GetAllAsync(TodoTaskSearchQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<TodoTask> queryable = _dbContext.Tasks.AsNoTracking();

            if(!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.Trim();

                queryable = queryable.Where(t => t.Name.Contains(searchTerm)
                    || (t.Description != null && t.Description.Contains(searchTerm)));
            }

            if (query.CreatedAfter.HasValue)
            {
                queryable = queryable.Where(t => t.CreatedAt > query.CreatedAfter.Value);
            }

            if (query.CreatedBefore.HasValue)
            {
                queryable = queryable.Where(t => t.CreatedAt < query.CreatedBefore.Value);
            }

            if (query.CompletionDateBefore.HasValue)
            {
                queryable = queryable.Where(t => t.DueDate < query.CompletionDateBefore.Value);
            }

            if (query.Status.HasValue)
            {
                queryable = queryable.Where(t => t.Status == query.Status.Value);
            }

            if (query.Priority.HasValue)
            {
                queryable = queryable.Where(t => t.Priority == query.Priority.Value);
            }

            queryable = query.SortBy?.ToLower() switch
            {
                "date" => query.IsDescending ? queryable.OrderByDescending(t => t.CreatedAt) : queryable.OrderBy(t => t.CreatedAt),
                "duedate" => query.IsDescending ? queryable.OrderByDescending(t => t.DueDate) : queryable.OrderBy(t => t.DueDate),
                "status" => query.IsDescending
                    ? queryable.OrderByDescending(t => t.Status == Model.Enums.TaskStatusEnum.Canceled ? 3 : t.Status == Model.Enums.TaskStatusEnum.Completed ? 2 : t.Status == Model.Enums.TaskStatusEnum.InProgress ? 1 : 0)
                    : queryable.OrderBy(t => t.Status == Model.Enums.TaskStatusEnum.Canceled ? 3 : t.Status == Model.Enums.TaskStatusEnum.Completed ? 2 : t.Status == Model.Enums.TaskStatusEnum.InProgress ? 1 : 0),
                "priority" => query.IsDescending 
                    ? queryable.OrderByDescending(t => t.Priority == Model.Enums.TaskPriorityEnum.High ? 2 : t.Priority == Model.Enums.TaskPriorityEnum.Medium ? 1 : 0) 
                    : queryable.OrderBy(t => t.Priority == Model.Enums.TaskPriorityEnum.High ? 2 : t.Priority == Model.Enums.TaskPriorityEnum.Medium ? 1 : 0),
                _ => queryable.OrderBy(t => t.Name)
            };

            queryable = queryable.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);

            return await queryable.ToListAsync(cancellationToken);
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
