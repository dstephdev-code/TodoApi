namespace TodoApp.Api.Model.TaskAssignment.Dto
{
    public record TaskAssignmentResponse(
            Guid Id,
            Guid TaskId,
            Guid UserId,
            Guid AssignedByUserId,
            DateTimeOffset AssignedAt,
            string Comment
    );
}
