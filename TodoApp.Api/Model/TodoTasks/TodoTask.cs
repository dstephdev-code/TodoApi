using TodoApp.Api.Model.Enums;
using TodoApp.Api.Model.TaskAssignment;

namespace TodoApp.Api.Model.TodoTasks
{
    public class TodoTask
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset DueDate { get; private set; }
        public DateTimeOffset? UpdatedAt { get; private set; }
        public TaskStatusEnum Status { get; private set; }
        public TaskPriorityEnum Priority { get; private set; }
        public ICollection<TaskAssignment.TaskAssignment> TaskAssignments { get; } = [];

        private TodoTask() { }
        public TodoTask(string name, string description, DateTimeOffset dueDate, TaskPriorityEnum priority)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            CreatedAt = DateTimeOffset.UtcNow;
            DueDate = dueDate;
            Status = TaskStatusEnum.Created;
            Priority = priority;
        }

        public void ChangeName(string? name)
        {
            if (name is null || name == Name) return;

            Name = name;
            SetUpdated();
        }
        public void ChangeDescription(string? description)
        {
            if (description is null || description == Description) return;

            Description = description;
            SetUpdated();
        }
        public void ChangeDueDate(DateTimeOffset? dueDate)
        {
            if (dueDate is null || dueDate.Value == DueDate) return;

            DueDate = dueDate.Value;
            SetUpdated();
        }
        public void ChangeStatus(TaskStatusEnum? status)
        {
            if (status is null || status.Value == Status) return;

            Status = status.Value;
            SetUpdated();
        }
        public void ChangePriority(TaskPriorityEnum? priority)
        {
            if (priority is null || priority.Value == Priority) return;

            Priority = priority.Value;
            SetUpdated();
        }
        public void AssignUser(User.User user, Guid assignedByUserId, string? comment)
        {
            if (!user.IsActive)
                throw new InvalidOperationException("User is inactive");

            if (TaskAssignments.Any(a => a.UserId == user.Id))
                throw new InvalidOperationException("User is already assigned!");

            TaskAssignments.Add(new TaskAssignment.TaskAssignment(Id, user.Id, assignedByUserId, comment));

            SetUpdated();
        }
        private void SetUpdated()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }

    }
}
