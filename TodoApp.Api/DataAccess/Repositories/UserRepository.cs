using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Model.TodoTasks;
using TodoApp.Api.Model.User;
using TodoApp.Api.Model.User.Dto;

namespace TodoApp.Api.DataAccess.Repositories
{
    public class UserRepository(TodoDbContext dbContext) : IUserRepository
    {
        private readonly TodoDbContext _dbContext = dbContext;
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<List<User>> GetAllAsync(GetUsersQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<User> queryable = _dbContext.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.Trim();

                queryable = queryable.Where(t => t.FirstName.Contains(searchTerm)
                                              || t.LastName.Contains(searchTerm) 
                                              || t.Email.Contains(searchTerm) 
                                              || t.Position.Contains(searchTerm));
            }

            if (query.CreatedAfter.HasValue)
            {
                queryable = queryable.Where(t => t.CreatedAt > query.CreatedAfter.Value);
            }

            if (query.CreatedBefore.HasValue)
            {
                queryable = queryable.Where(t => t.CreatedAt < query.CreatedBefore.Value);
            }

            if (query.IsActive)
            {
                queryable = queryable.Where(t => t.IsActive == true);
            }

            queryable = query.SortBy?.ToLower() switch
            {
                "firstname" => query.IsDescending ? queryable.OrderByDescending(t => t.FirstName) : queryable.OrderBy(t => t.FirstName),
                "lastname" => query.IsDescending ? queryable.OrderByDescending(t => t.LastName) : queryable.OrderBy(t => t.LastName),
                "email" => query.IsDescending ? queryable.OrderByDescending(t => t.Email) : queryable.OrderBy(t => t.Email),
                "position" => query.IsDescending ? queryable.OrderByDescending(t => t.Position) : queryable.OrderBy(t => t.Position),
                "date" => query.IsDescending ? queryable.OrderByDescending(t => t.CreatedAt) : queryable.OrderBy(t => t.CreatedAt),
                _ => queryable.OrderBy(t => t.FirstName)
            };

            queryable = queryable.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);

            return await queryable.ToListAsync(cancellationToken);
        }
        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
        }
        public void Remove(User user)
        {
            _dbContext.Users.Remove(user);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
