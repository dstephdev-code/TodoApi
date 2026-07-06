using TodoApp.Api.Exceptions;
using TodoApp.Api.Model.Enums;

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

        public bool IsLate =>
            DueDate < DateTimeOffset.UtcNow &&
            Status != TaskStatusEnum.Completed &&
            Status != TaskStatusEnum.Canceled;

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
            EnsureTaskIsEditable();
            if (name is null || name == Name) return;

            Name = name;
            SetUpdated();
        }
        public void ChangeDescription(string? description)
        {
            EnsureTaskIsEditable();
            if (description is null || description == Description) return;

            Description = description;
            SetUpdated();
        }
        public void ChangeDueDate(DateTimeOffset? dueDate)
        {
            EnsureTaskIsEditable();
            if (dueDate is null || dueDate.Value == DueDate) return;

            DueDate = dueDate.Value;
            SetUpdated();
        }
        public void ChangePriority(TaskPriorityEnum? priority)
        {
            EnsureTaskIsEditable();
            if (priority is null || priority.Value == Priority) return;

            Priority = priority.Value;
            SetUpdated();
        }
        public void AssignUser(User.User user, Guid assignedByUserId, string? comment)
        {
            EnsureTaskIsEditable();
            if (!user.IsActive)
                throw new DomainException("User is inactive.");
            if (TaskAssignments.Any(a => a.UserId == user.Id))
                throw new DomainException("User is already assigned.");

            TaskAssignments.Add(new TaskAssignment.TaskAssignment(Id, user.Id, assignedByUserId, comment));
            SetUpdated();
        }
        public void UnassignUser(User.User user)
        {
            EnsureTaskIsEditable();
            var taskAssignment = TaskAssignments.FirstOrDefault(a => a.UserId == user.Id) ??
                throw new DomainException("User is not assigned to this task.");

            TaskAssignments.Remove(taskAssignment);
            SetUpdated();
        }
        public void Start()
        {
            if (Status is not TaskStatusEnum.Created) throw new DomainException("Only newly created tasks can be started.");
            if (TaskAssignments.Count == 0) throw new DomainException("No users assigned to the task.");

            Status = TaskStatusEnum.InProgress;
            SetUpdated();
        }
        public void Complete()
        {
            if (Status is not TaskStatusEnum.InProgress) throw new DomainException("Only tasks in progress can be completed.");

            Status = TaskStatusEnum.Completed;
            SetUpdated();
        }
        public void Cancel()
        {
            EnsureTaskIsEditable();

            Status = TaskStatusEnum.Canceled;
            SetUpdated();
        }

        private void SetUpdated()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }
        private void EnsureTaskIsEditable()
        {
            if (Status is TaskStatusEnum.Completed or TaskStatusEnum.Canceled)
                throw new DomainException("Task is already completed or canceled.");
        }
    }
}