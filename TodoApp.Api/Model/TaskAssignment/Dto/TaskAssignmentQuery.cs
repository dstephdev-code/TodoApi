using TodoApp.Api.Model.Enums;

namespace TodoApp.Api.Model.TaskAssignment.Dto
{
    public record TaskAssignmentQuery
    {
        public string? SearchTerm { get; init; }

        public Guid? TaskId { get; init; }
        public Guid? UserId { get; init; }
        public Guid? AssignedById { get; init; }
        public DateTimeOffset? AssignedBefore { get; init; }
        public DateTimeOffset? AssignedAfter { get; init; }

        public TaskAssignmentSortField? SortBy { get; init; }
        public bool IsDescending { get; init; }

        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
