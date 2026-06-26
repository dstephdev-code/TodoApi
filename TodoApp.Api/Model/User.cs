namespace TodoApp.Api.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public List<TaskAssignment> TaskAssignments { get; set; } = [];
    }
}
