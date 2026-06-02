namespace TodoApi.Model
{
    public class TaskAssignment
    {
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }
        public User User { get; set; } = null!;
        public TodoTask Task { get; set; } = null!;
    }
}
