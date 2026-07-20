using TodoApp.Api.DataAccess.Repositories;
using TodoApp.Api.Exceptions;
using TodoApp.Api.Model.TaskAssignment;
using TodoApp.Api.Model.TaskAssignment.Dto;

namespace TodoApp.Api.Services
{
    public class TaskAssignmentService(ITaskAssignmentRepository taskAssignmentRepository) : ITaskAssignmentService
    {
        private readonly ITaskAssignmentRepository _taskAssignmentRepository = taskAssignmentRepository;

        public async Task<TaskAssignmentResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var taskAssignment = await GetOrThrowAsync(id, cancellationToken);

            return MapToDto(taskAssignment);
        }

        public async Task<IEnumerable<TaskAssignmentResponse>> SearchAsync(TaskAssignmentQuery query, CancellationToken cancellationToken = default)
        {
            var taskAssignments = await _taskAssignmentRepository.GetAllAsync(query, cancellationToken);

            return taskAssignments.Select(MapToDto);
        }

        private async Task<TaskAssignment> GetOrThrowAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var taskAssignment = await _taskAssignmentRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException($"Task assignment by id {id} was not found!");
            return taskAssignment;
        }

        private static TaskAssignmentResponse MapToDto(TaskAssignment taskAssignment) =>
            new(taskAssignment.TaskId, taskAssignment.TaskId, taskAssignment.UserId, taskAssignment.AssignedByUserId, taskAssignment.AssignedAt, taskAssignment.Comment);
    }
}
