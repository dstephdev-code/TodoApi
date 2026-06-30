using TodoApp.Api.Model.Enums;

namespace TodoApp.Api.Model
{
    public class TodoTaskSearchQuery
    {
        public string? SearchTerm { get; set; }
        public DateTimeOffset? CreatedAfter { get; set; }
        public DateTimeOffset? CreatedBefore { get; set; }
        public DateTimeOffset? CompletionDateBefore { get; set; }
        public TaskStatusEnum? Status { get; set; }
        public TaskPriorityEnum? Priority { get; set; }

        public string? SortBy { get; set; }
        public bool IsDescending { get; set; } = false;

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
