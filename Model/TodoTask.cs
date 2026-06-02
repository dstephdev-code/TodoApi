using TodoApi.Model.Enums;

namespace TodoApi.Model
{
    public class TodoTask
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public TaskStatusEnum Status { get; set; }
        public TaskPriorityEnum Priority { get; set; }
        public List<TaskAssignment> TaskAssignment { get; set; } = [];

    }
}
