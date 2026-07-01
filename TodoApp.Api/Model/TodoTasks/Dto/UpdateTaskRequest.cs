using TodoApp.Api.Model.Enums;

namespace TodoApp.Api.Model.TodoTasks.Dto
{
    public record UpdateTaskRequest(
            string? Name,
            string? Description,
            DateTimeOffset? DueDate,
            TaskStatusEnum? Status,
            TaskPriorityEnum? Priority
    );
}
