using TodoApp.Api.Model.TodoTasks;

namespace TodoApp.Api.Model.TaskAssignment
{
    public class TaskAssignment
    {
        public Guid Id { get; private set; }
        public Guid TaskId { get; private set; }
        public Guid UserId { get; private set; }
        public TodoTask Task { get; private set; } = null!;
        public User.User User { get; private set; } = null!;
        public Guid AssignedByUserId { get; private set; }
        public User.User AssignedByUser { get; private set; } = null!;
        public DateTimeOffset AssignedAt { get; private set; }
        public string Comment { get; private set; } = null!;

        private TaskAssignment() { }

        public TaskAssignment(Guid taskId, Guid userId, Guid assignedByUserId, string? comment = null)
        {
            Id = Guid.NewGuid();
            TaskId = taskId;
            UserId = userId;
            AssignedByUserId = assignedByUserId;
            AssignedAt = DateTimeOffset.UtcNow;
            Comment = comment ?? string.Empty;
        }

    }
}
