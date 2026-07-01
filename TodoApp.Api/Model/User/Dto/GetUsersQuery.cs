namespace TodoApp.Api.Model.User.Dto
{
    public record GetUsersQuery
    {
        public string? SearchTerm { get; init; }
        public DateTimeOffset? CreatedAfter { get; init; }
        public DateTimeOffset? CreatedBefore { get; init; }
        public bool IsActive { get; init; } = true;

        public string? SortBy { get; init; }
        public bool IsDescending { get; init; } = false;

        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
