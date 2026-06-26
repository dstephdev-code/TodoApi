using TodoApp.Api.Model.Enums;

namespace TodoApp.Api.Model
{
    public class TodoTaskCreateDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public TaskPriorityEnum Priority { get; set; }
    }
}
