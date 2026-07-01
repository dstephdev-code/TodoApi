using TodoApp.Api.Model.Enums;
using TodoApp.Api.Model.TodoTasks;
using Xunit.Internal;

namespace TodoApp.Tests
{
    public class TodoTaskEntityTests
    {

        [Fact]
        public void ChangeName_WhenValidName_ChangesNameAndUpdatesUpdatedAt()
        {
            TodoTask task = new("Test task", "Description", DateTimeOffset.UtcNow.AddDays(1), TaskPriorityEnum.Medium);

            task.ChangeName("New task name");

            Assert.Equal("New task name", task.Name);
            Assert.NotNull(task.UpdatedAt);
        }

        [Fact]
        public void ChangePriority_WhenNull_DoesNothing()
        {
            TodoTask task = new("Test task", "Description", DateTimeOffset.UtcNow.AddDays(1), TaskPriorityEnum.Medium);

            task.ChangePriority(null);

            Assert.Equal(TaskPriorityEnum.Medium, task.Priority);
            Assert.Null(task.UpdatedAt);
        }

        [Fact]
        public void ChangePriority_WhenPriorityIsSame_DoesNothing()
        {
            TodoTask task = new("Test task", "Description", DateTimeOffset.UtcNow.AddDays(1), TaskPriorityEnum.Medium);

            task.ChangePriority(TaskPriorityEnum.Medium);

            Assert.Equal(TaskPriorityEnum.Medium, task.Priority);
            Assert.Null(task.UpdatedAt);
        }

        [Fact]
        public void ChangePriority_WhenPriorityChanged_UpdatesPriorityAndUpdatedAt()
        {
            TodoTask task = new("Test task", "Description", DateTimeOffset.UtcNow.AddDays(1), TaskPriorityEnum.Medium);

            task.ChangePriority(TaskPriorityEnum.High);

            Assert.Equal(TaskPriorityEnum.High, task.Priority);
            Assert.NotNull(task.UpdatedAt);
        }
    }
}
