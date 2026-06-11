using TodoApi.Model.Enums;

namespace TodoApi.Model
{
    public class TodoTaskCreateDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public TaskPriorityEnum Priority { get; set; }
    }
}
