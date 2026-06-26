using TodoApp.Api.Model.Enums;

namespace TodoApp.Api.Model
{
    public class TodoTaskUpdateDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public TaskStatusEnum? Status { get; set; }
        public TaskPriorityEnum? Priority { get; set; }
    }
}
