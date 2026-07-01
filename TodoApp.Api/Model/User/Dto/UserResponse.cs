namespace TodoApp.Api.Model.User.Dto
{
    public record UserResponse(
            Guid Id,
            string FirstName,
            string LastName,
            string Email,
            string Position,
            DateTimeOffset CreatedAt,
            bool IsActive
        );
}
    