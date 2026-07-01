using TodoApp.Api.Model.User;
using TodoApp.Api.Model.User.Dto;

namespace TodoApp.Api.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<User>> GetAllAsync(GetUsersQuery query, CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        void Remove(User user);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
