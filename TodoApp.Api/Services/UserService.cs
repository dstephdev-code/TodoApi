using TodoApp.Api.DataAccess.Repositories;
using TodoApp.Api.Exceptions;
using TodoApp.Api.Model.User;
using TodoApp.Api.Model.User.Dto;

namespace TodoApp.Api.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<UserResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await GetOrThrowAsync(id, cancellationToken);

            return MapToDto(user);
        }
        public async Task<IEnumerable<UserResponse>> GetAllAsync(GetUsersQuery query, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetAllAsync(query, cancellationToken);

            return user.Select(MapToDto);
        }
        public async Task<UserResponse> CreateAsync(CreateUserRequest userDto, CancellationToken cancellationToken = default)
        {
            var user = new User(userDto.FirstName, userDto.LastName, userDto.Email, userDto.Position);

            await _userRepository.AddAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            return MapToDto(user);
        }
        public async Task RemoveById(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await GetOrThrowAsync(id, cancellationToken);
            // I'm not deleting it right away because we need first to check if we have connected tasks.
            // For now there is no check for it, I simply checking null, but its temporary
            _userRepository.Remove(user);

            await _userRepository.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdatePartialAsync(Guid id, UpdateUserRequest userDto, CancellationToken cancellationToken = default)
        {
            var user = await GetOrThrowAsync(id, cancellationToken);
            user.ChangeFirstName(userDto.FirstName);
            user.ChangeLastName(userDto.LastName);
            user.ChangeEmail(userDto.Email);
            user.ChangePosition(userDto.Position);

            await _userRepository.SaveChangesAsync(cancellationToken);
        }

        private async Task<User> GetOrThrowAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException($"User by id {id} was not found!");
            return user;
        }
        private static UserResponse MapToDto(User user) =>
            new(user.Id, user.FirstName, user.LastName, user.Email, user.Position, user.CreatedAt, user.IsActive);

    }
}
