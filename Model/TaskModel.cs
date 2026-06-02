namespace TodoApi.Model
{
    public class TaskModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateComplitionExpected { get; set; }
        public List<UserImplementor>? ImplementorsList { get; set; } = [];
        public DateTime? DateUpdated { get; set; }
        public decimal? CurrentStatus { get; set; }

    }
}
