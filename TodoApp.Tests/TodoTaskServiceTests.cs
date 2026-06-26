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
    }
}
