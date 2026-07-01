using TodoApp.Api.Model.Enums;

namespace TodoApp.Api.Model.TodoTasks.Dto
{
    public record CreateTaskRequest(
            string Name,
            string Description,
            DateTimeOffset DueDate,
            TaskPriorityEnum Priority
    );
}
