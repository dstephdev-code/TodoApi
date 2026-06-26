using TodoApp.Api.Model.Enums;

namespace TodoApp.Api.Model
{
    public class TodoTaskSearchQuery
    {
        public string? SearchTerm { get; set; }

        public TaskStatusEnum? Status { get; set; }
        public TaskPriorityEnum? Priority { get; set; }

        public string? SortBy { get; set; }
        public bool IsDescending { get; set; } = false;
    }
}
