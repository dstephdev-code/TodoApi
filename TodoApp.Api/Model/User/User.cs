namespace TodoApp.Api.Model.User
{
    public class User
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string Position { get; private set; } = null!;
        public DateTimeOffset CreatedAt { get; private set; }
        public bool IsActive { get; private set; }
        public ICollection<TaskAssignment> TaskAssignments { get; } = [];

        public string FullName => $"{FirstName} {LastName}";

        private User() { }
        public User(string firstName, string lastName, string email, string position)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Position = position;
            CreatedAt = DateTimeOffset.UtcNow;
            IsActive = true;
        }
        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
