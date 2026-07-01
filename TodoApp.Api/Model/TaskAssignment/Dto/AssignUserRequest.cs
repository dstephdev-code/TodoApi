namespace TodoApp.Api.Model.TaskAssignment.Dto
{
    public record AssignUserRequest(
                Guid UserId,
                Guid AssignedByUserId,
                string? Comment
        );
}
