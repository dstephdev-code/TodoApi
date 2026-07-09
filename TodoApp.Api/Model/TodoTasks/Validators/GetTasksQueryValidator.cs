using FluentValidation;
using TodoApp.Api.Model.TodoTasks.Dto;

namespace TodoApp.Api.Model.TodoTasks.Validators
{
    public class GetTasksQueryValidator : AbstractValidator <GetTasksQuery>
    {
        public GetTasksQueryValidator()
        {
            RuleFor(x => x.SearchTerm)
                .Length(1, 256).WithMessage("Search term length must be between 1 and 256 characters.")
                .When(x => x.SearchTerm != null);

            RuleFor(x => x.CreatedAfter)
                .LessThan(DateTimeOffset.UtcNow).WithMessage("Created after date cannot be in the future.")
                .When(x => x.CreatedAfter != null);
            RuleFor(x => x.CreatedBefore)
                .LessThan(DateTimeOffset.UtcNow).WithMessage("Created before date cannot be in the future.")
                .When(x => x.CreatedAfter != null);

            RuleFor(x => x)
                .Must(x =>
                    !x.CreatedAfter.HasValue ||
                    !x.CreatedBefore.HasValue ||
                    x.CreatedAfter <= x.CreatedBefore)
                .WithMessage("'CreatedAfter' must be earlier than 'CreatedBefore'.");
            RuleFor(x => x)
                .Must(x =>
                    !x.DueAfter.HasValue ||
                    !x.DueBefore.HasValue ||
                    x.DueAfter <= x.DueBefore)
                .WithMessage("'DueAfter' must be earlier than 'DueBefore'.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status value."); 
            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Invalid priority value.");

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
