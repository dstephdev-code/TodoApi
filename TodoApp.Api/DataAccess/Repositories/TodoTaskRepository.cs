using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Model.Enums;
using TodoApp.Api.Model.TodoTasks;
using TodoApp.Api.Model.TodoTasks.Dto;

namespace TodoApp.Api.DataAccess.Repositories
{
    public class TodoTaskRepository(TodoDbContext context) : ITodoTaskRepository
    {
        private readonly TodoDbContext _dbContext = context;

        public async Task<TodoTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Tasks
                .Include(t => t.TaskAssignments)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<List<TodoTask>> GetAllAsync(GetTasksQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<TodoTask> queryable = _dbContext.Tasks.AsNoTracking();

            if(!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.Trim();
                queryable = queryable.Where(t => 
                    t.Name.Contains(searchTerm) || t.Description.Contains(searchTerm));
            }

            if (query.CreatedAfter.HasValue)
                queryable = queryable.Where(t => t.CreatedAt > query.CreatedAfter.Value);
            if (query.CreatedBefore.HasValue)
                queryable = queryable.Where(t => t.CreatedAt < query.CreatedBefore.Value);

            if (query.DueAfter.HasValue)
                queryable = queryable.Where(t => t.DueDate > query.DueAfter.Value);
            if (query.DueBefore.HasValue)
                queryable = queryable.Where(t => t.DueDate < query.DueBefore.Value);

            if (query.Status.HasValue)
                queryable = queryable.Where(t => t.Status == query.Status.Value);
            if (query.Priority.HasValue)
                queryable = queryable.Where(t => t.Priority == query.Priority.Value);

            ApplySorting(queryable, query);

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

        private static IQueryable<TodoTask> ApplySorting(IQueryable<TodoTask> queryable, GetTasksQuery query)
        {
            return query.SortBy switch
            {
                TaskSortField.CreatedAt => query.IsDescending ? queryable.OrderByDescending(t => t.CreatedAt) : queryable.OrderBy(t => t.CreatedAt),
                TaskSortField.DueDate => query.IsDescending ? queryable.OrderByDescending(t => t.DueDate) : queryable.OrderBy(t => t.DueDate),
                TaskSortField.Status => query.IsDescending
                    ? queryable.OrderByDescending(t => t.Status == TaskStatusEnum.Canceled ? 3 : t.Status == TaskStatusEnum.Completed ? 2 : t.Status == TaskStatusEnum.InProgress ? 1 : 0)
                    : queryable.OrderBy(t => t.Status == TaskStatusEnum.Canceled ? 3 : t.Status == TaskStatusEnum.Completed ? 2 : t.Status == TaskStatusEnum.InProgress ? 1 : 0),
                TaskSortField.Priority => query.IsDescending
                    ? queryable.OrderByDescending(t => t.Priority == TaskPriorityEnum.High ? 2 : t.Priority == TaskPriorityEnum.Medium ? 1 : 0)
                    : queryable.OrderBy(t => t.Priority == TaskPriorityEnum.High ? 2 : t.Priority == TaskPriorityEnum.Medium ? 1 : 0),
                _ => queryable.OrderBy(t => t.Name)
            };
        }
    }
}
