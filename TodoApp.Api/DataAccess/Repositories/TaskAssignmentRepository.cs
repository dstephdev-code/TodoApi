using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Model.Enums;
using TodoApp.Api.Model.TaskAssignment;
using TodoApp.Api.Model.TaskAssignment.Dto;

namespace TodoApp.Api.DataAccess.Repositories
{
    public class TaskAssignmentRepository(TodoDbContext dbContext) : ITaskAssignmentRepository
    {
        private readonly TodoDbContext _dbContext = dbContext;
        public async Task<TaskAssignment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.TaskAssignments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<List<TaskAssignment>> GetAllAsync(TaskAssignmentQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<TaskAssignment> queryable = _dbContext.TaskAssignments.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.Trim();
                queryable = queryable.Where(ta =>
                    ta.Comment.Contains(searchTerm));
            }

            if (query.AssignedAfter.HasValue)
                queryable = queryable.Where(ta => ta.AssignedAt > query.AssignedAfter.Value);
            if (query.AssignedBefore.HasValue)
                queryable = queryable.Where(ta => ta.AssignedAt < query.AssignedBefore.Value);

            if (query.UserId.HasValue)
                queryable = queryable.Where(ta => ta.UserId == query.UserId.Value);
            if (query.TaskId.HasValue)
                queryable = queryable.Where(ta => ta.TaskId == query.TaskId.Value);
            if (query.AssignedById.HasValue)
                queryable = queryable.Where(ta => ta.AssignedByUserId == query.AssignedById.Value);

            ApplySorting(queryable, query);

            queryable = queryable.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);

            return await queryable.ToListAsync(cancellationToken);
        }
        private static IQueryable<TaskAssignment> ApplySorting(IQueryable<TaskAssignment> queryable, TaskAssignmentQuery query)
        {
            return query.SortBy switch
            {
                TaskAssignmentSortField.AssignDate => query.IsDescending ? queryable.OrderByDescending(ta => ta.AssignedAt) : queryable.OrderBy(ta => ta.AssignedAt),
                TaskAssignmentSortField.AssignBy => query.IsDescending ? queryable.OrderByDescending(ta => ta.AssignedByUserId) : queryable.OrderBy(ta => ta.AssignedByUserId),
                TaskAssignmentSortField.UserId => query.IsDescending ? queryable.OrderByDescending(ta => ta.UserId) : queryable.OrderBy(ta => ta.UserId),
                TaskAssignmentSortField.TaskId => query.IsDescending ? queryable.OrderByDescending(ta => ta.TaskId) : queryable.OrderBy(ta => ta.TaskId),
                _ => queryable.OrderBy(ta => ta.AssignedAt)
            };
        }
    }
}
