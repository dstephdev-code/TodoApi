using Microsoft.Extensions.Logging;
using NSubstitute;
using TodoApp.Api.DataAccess.Repositories;
using TodoApp.Api.Services;
using TodoApp.Api.Model;
using TodoApp.Api.Model.Enums;
using TodoApp.Api.Exceptions;

namespace TodoApp.Tests
{
    public class TodoTaskServiceTests
    {
        private readonly ITodoTaskRepository _repositoryMock;
        private readonly ILogger<TodoTasksService> _loggerMock;
        private readonly TodoTasksService _service;

        public TodoTaskServiceTests()
        {
            _repositoryMock = Substitute.For<ITodoTaskRepository>();
            _loggerMock = Substitute.For<ILogger<TodoTasksService>>();
            _service = new TodoTasksService(_repositoryMock, _loggerMock);
        }

        [Fact]
        public async Task GetByIdAsync_WhenTaskExists_ReturnsMappesTaskDTO()
        {
            var taskId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var existingTask = new TodoTask
            {
                Id = taskId,
                Name = "Test Task",
                Description = "Test Description",
                Status = TaskStatusEnum.Created,
                Priority = TaskPriorityEnum.High,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _repositoryMock.GetByIdAsync(taskId, cancellationToken).Returns(existingTask);

            var result = await _service.GetByIdAsync(taskId, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
            Assert.Equal("Test Task", result.Name);
            Assert.Equal("Test Description", result.Description);
            Assert.Equal(TaskPriorityEnum.High, result.Priority);

            await _repositoryMock.Received(1).GetByIdAsync(taskId, cancellationToken);
        }

        [Fact]
        public async Task GetByIdAsync_WhenTaskDoesNotExist_ThrowsNotFoundException()
        {
            var taskId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            _repositoryMock.GetByIdAsync(taskId, cancellationToken).Returns((TodoTask?)null);

            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.GetByIdAsync(taskId, cancellationToken));

            Assert.Equal($"ToDo task by id {taskId} was not found!", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WithValidDTO_SavesTaskAndReturnsDTO()
        {
            var cancellationToken = CancellationToken.None;
            var createDTO = new TodoTaskCreateDTO
            {
                Name = "New Task",
                Description = "Description",
                Priority = TaskPriorityEnum.Medium,
                DueDate = DateTimeOffset.UtcNow.AddDays(1)
            };

            var result = await _service.CreateAsync(createDTO, cancellationToken);

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(createDTO.Name, result.Name);
            Assert.Equal(TaskStatusEnum.Created, result.Status);

            await _repositoryMock.Received(1).AddAsync(Arg.Any<TodoTask>(), cancellationToken);
            await _repositoryMock.Received(1).SaveChangesAsync(cancellationToken);
        }

        [Fact]
        public async Task GetAllAsync_WithFilters_ReturnsMappedDTOCollection()
        {
            var cancellationToken = CancellationToken.None;

            var query = new TodoTaskSearchQuery
            {
                SearchTerm = "Fix",
                Status = TaskStatusEnum.Created,
                Priority = TaskPriorityEnum.High,
                SortBy = "priority",
                IsDescending = true
            };

            var repoResult = new List<TodoTask>
            {
                new TodoTask
                {
                    Id = Guid.NewGuid(),
                    Name = "Fix bug",
                    Description = "There is a bug",
                    Status = TaskStatusEnum.Created,
                    Priority = TaskPriorityEnum.High
                }
            };

            _repositoryMock.GetAllAsync(query, cancellationToken).Returns(repoResult);

            var result = await _service.GetAllAsync(query, cancellationToken);

            Assert.NotNull(result);
            var resultList = Assert.IsAssignableFrom<IEnumerable<TodoTaskDTO>>(result);
            var singleDTO = Assert.Single(resultList);

            Assert.Equal(repoResult[0].Id, singleDTO.Id);
            Assert.Equal("Fix bug", singleDTO.Name);
            Assert.Equal("There is a bug", singleDTO.Description);
            Assert.Equal(TaskStatusEnum.Created, singleDTO.Status);
            Assert.Equal(TaskPriorityEnum.High, singleDTO.Priority);

            await _repositoryMock.Received(1).GetAllAsync(query, cancellationToken);
        }

        [Fact]
        public async Task GetAllAsync_WithEmptyFilters_ReturnsAllTasks()
        {
            var cancellationToken = CancellationToken.None;
            var emptyQuery = new TodoTaskSearchQuery();

            var repoResult = new List<TodoTask>
            {
                new TodoTask { Id = Guid.NewGuid(), Name = "Task 1", Description = "Task 1 description" },
                new TodoTask { Id = Guid.NewGuid(), Name = "Task 2", Description = "Task 2 description" }
            };

            _repositoryMock.GetAllAsync(emptyQuery, cancellationToken).Returns(repoResult);

            var result = await _service.GetAllAsync(emptyQuery, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            await _repositoryMock.Received(1).GetAllAsync(emptyQuery, cancellationToken);
        }
    }
}
