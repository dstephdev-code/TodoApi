using TodoApp.Api.Model.Enums;

namespace TodoApp.Api.Model
{
    public class TodoTaskSearchQuery
    {
        private const int MAX_PAGE_SIZE = 100;
        private const int DEFAULT_PAGE_SIZE = 10;

        public string? SearchTerm { get; set; }

        public TaskStatusEnum? Status { get; set; }
        public TaskPriorityEnum? Priority { get; set; }

        public string? SortBy { get; set; }
        public bool IsDescending { get; set; } = false;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = DEFAULT_PAGE_SIZE;
        public int PageSize
        {
            get => _pageSize; 
            set
            {
                _pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
            }
        }

        // Add validation!!!
    }
}
