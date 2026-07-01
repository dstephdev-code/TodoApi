using Microsoft.Extensions.Logging;
using NSubstitute;
using TodoApp.Api.DataAccess.Repositories;
using TodoApp.Api.Services;
using TodoApp.Api.Model;
using TodoApp.Api.Model.Enums;
using TodoApp.Api.Exceptions;
using TodoApp.Api.Model.TodoTasks;
using TodoApp.Api.Model.TodoTasks.Dto;

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

        private static TodoTask CreateTask(string name = "Test name") => 
            new(name, "Task description", DateTimeOffset.UtcNow.AddDays(1), TaskPriorityEnum.Low);

        [Fact]
        public async Task GetByIdAsync_WhenTaskExists_ReturnsMappedTaskResponse()
        {
            var cancellationToken = CancellationToken.None;

            var existingTask = CreateTask();
            var taskId = existingTask.Id;

            _repositoryMock.GetByIdAsync(taskId, cancellationToken).Returns(existingTask);

            var result = await _service.GetByIdAsync(taskId, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
            Assert.Equal("Test name", result.Name);
            Assert.Equal("Task description", result.Description);
            Assert.Equal(TaskPriorityEnum.Low, result.Priority);

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
            var createRequest = new CreateTaskRequest("New Task", "Description", DateTimeOffset.UtcNow.AddDays(1), TaskPriorityEnum.Medium);

            var result = await _service.CreateAsync(createRequest, cancellationToken);

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(createRequest.Name, result.Name);
            Assert.Equal(TaskStatusEnum.Created, result.Status);

            await _repositoryMock.Received(1).AddAsync(Arg.Any<TodoTask>(), cancellationToken);
            await _repositoryMock.Received(1).SaveChangesAsync(cancellationToken);
        }

        [Fact]
        public async Task GetAllAsync_WithFilters_ReturnsMappedCollection()
        {
            var cancellationToken = CancellationToken.None;

            var query = new GetTasksQuery
            {
                SearchTerm = "Fix",
                Status = TaskStatusEnum.Created,
                Priority = TaskPriorityEnum.Low,
                SortBy = "priority",
                IsDescending = true
            };

            var repoResult = new List<TodoTask>
            {
                CreateTask("Fix bug")
            };

            _repositoryMock.GetAllAsync(query, cancellationToken).Returns(repoResult);

            var result = await _service.GetAllAsync(query, cancellationToken);

            Assert.NotNull(result);
            var resultEnumerable = Assert.IsType<IEnumerable<TaskResponse>>(result, exactMatch: false);
            var singleResponse = Assert.Single(resultEnumerable);

            Assert.Equal(repoResult[0].Id, singleResponse.Id);
            Assert.Equal("Fix bug", singleResponse.Name);
            Assert.Equal("Task description", singleResponse.Description);
            Assert.Equal(TaskStatusEnum.Created, singleResponse.Status);
            Assert.Equal(TaskPriorityEnum.Low, singleResponse.Priority);

            await _repositoryMock.Received(1).GetAllAsync(query, cancellationToken);
        }

        [Fact]
        public async Task GetAllAsync_WithEmptyFilters_ReturnsAllTasks()
        {
            var cancellationToken = CancellationToken.None;
            var emptyQuery = new GetTasksQuery();

            var repoResult = new List<TodoTask>
            {
                CreateTask("Task 1"),
                CreateTask("Task 2")
            };

            _repositoryMock.GetAllAsync(emptyQuery, cancellationToken).Returns(repoResult);

            var result = await _service.GetAllAsync(emptyQuery, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            await _repositoryMock.Received(1).GetAllAsync(emptyQuery, cancellationToken);
        }
    }
}
