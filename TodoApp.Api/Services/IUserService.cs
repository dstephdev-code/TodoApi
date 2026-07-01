using TodoApp.Api.Model.User.Dto;

namespace TodoApp.Api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync(GetUsersQuery query, CancellationToken cancellationToken = default);
        Task<UserResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<UserResponse> CreateAsync(CreateUserRequest userDto, CancellationToken cancellationToken = default);
        Task RemoveById(Guid id, CancellationToken cancellationToken = default);
        Task UpdatePartialAsync(Guid id, UpdateUserRequest userDto, CancellationToken cancellationToken = default);
    }
}
