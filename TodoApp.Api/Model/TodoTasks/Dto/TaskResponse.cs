using TodoApp.Api.Model.Enums;

namespace TodoApp.Api.Model.TodoTasks.Dto
{
    public record TaskResponse(
            Guid Id,
            string Name,
            string Description,
            DateTimeOffset CreatedAt,
            DateTimeOffset DueDate,
            DateTimeOffset? UpdatedAt,
            TaskStatusEnum Status,
            TaskPriorityEnum Priority
    );
}
