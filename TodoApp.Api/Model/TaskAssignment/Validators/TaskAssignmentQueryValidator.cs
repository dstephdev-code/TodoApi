using FluentValidation;
using TodoApp.Api.Model.TaskAssignment.Dto;

namespace TodoApp.Api.Model.TaskAssignment.Validators
{
    public class TaskAssignmentQueryValidator : AbstractValidator<TaskAssignmentQuery>
    {
        public TaskAssignmentQueryValidator()
        {
            RuleFor(x => x.SearchTerm)
                .Length(1, 256).WithMessage("Search term length must be between 1 and 256 characters.")
                .When(x => x.SearchTerm != null);

            RuleFor(x => x.AssignedAfter)
                .LessThan(DateTimeOffset.UtcNow).WithMessage("Assigned After date cannot be in the future.")
                .When(x => x.AssignedAfter != null);
            RuleFor(x => x.AssignedBefore)
                .LessThan(DateTimeOffset.UtcNow).WithMessage("Assigned Before date cannot be in the future.")
                .When(x => x.AssignedBefore != null);

            RuleFor(x => x)
                .Must(x =>
                    !x.AssignedAfter.HasValue ||
                    !x.AssignedBefore.HasValue ||
                    x.AssignedAfter <= x.AssignedBefore)
                .WithMessage("'AssignedAfter' must be earlier than 'AssignedBefore'.");

            RuleFor(x => x.SortBy)
                .IsInEnum().WithMessage("Invalid sorting field.");

            RuleFor(x => x.PageSize)
                .NotEmpty().WithMessage("Dont leave page size empty.")
                .GreaterThan(0).WithMessage("Page size must be greater than zero.")
                .LessThanOrEqualTo(100).WithMessage("Page size must be lesser than hundred.");

            RuleFor(x => x.PageNumber)
                .NotEmpty().WithMessage("Dont leave page number empty.")
                .GreaterThan(0).WithMessage("Page number must be greater than zero.");
        }
    }
}
