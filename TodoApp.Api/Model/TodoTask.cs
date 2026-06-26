using TodoApp.Api.Model.Enums;

namespace TodoApp.Api.Model
{
    public class TodoTask
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public TaskStatusEnum Status { get; set; }
        public TaskPriorityEnum Priority { get; set; }
        public List<TaskAssignment> TaskAssignments { get; set; } = [];

    }
}
