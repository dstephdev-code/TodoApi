namespace TodoApp.Api.Model.User.Dto
{
    public record UpdateUserRequest(
            string? FirstName,
            string? LastName,
            string? Email,
            string? Position
        );
}
