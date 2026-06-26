using TodoApi.Model.Enums;

namespace TodoApi.Model
{
    public class TodoTaskDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public TaskStatusEnum Status { get; set; }
        public TaskPriorityEnum Priority { get; set; }
    }
}
