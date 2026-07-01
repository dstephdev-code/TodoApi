namespace TodoApp.Api.Model.User.Dto
{
    public record CreateUserRequest(
            string FirstName,
            string LastName,
            string Email,
            string Position
    );
}
