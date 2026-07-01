using TodoApp.Api.Model.Enums;

namespace TodoApp.Api.Model.TodoTasks.Dto
{
    public record GetTasksQuery
    {
        public string? SearchTerm { get; init; }
        public DateTimeOffset? CreatedAfter { get; init; }
        public DateTimeOffset? CreatedBefore { get; init; }
        public DateTimeOffset? CompletionDateBefore { get; init; }
        public TaskStatusEnum? Status { get; init; }
        public TaskPriorityEnum? Priority { get; init; }

        public string? SortBy { get; init; }
        public bool IsDescending { get; init; } = false;

        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
