using FluentValidation;
using TodoApp.Api.Model.TodoTasks.Dto;

namespace TodoApp.Api.Model.TodoTasks.Validators
{
    public class GetTasksQueryValidator : AbstractValidator <GetTasksQuery>
    {
        public GetTasksQueryValidator()
        {
            RuleFor(x => x.SearchTerm)
                .Length(1, 256).WithMessage("Search term length should be between 1 and 256 symbols")
                .When(x => x.SearchTerm != null);

            RuleFor(x => x.CreatedAfter)
                .NotEmpty().WithMessage("If you pass 'created after' property dont leave it empty")
                .LessThan(DateTimeOffset.UtcNow).WithMessage("There is no tasks created in future")
                .When(x => x.CreatedAfter != null);

            RuleFor(x => x.CreatedBefore)
                .NotEmpty().WithMessage("If you pass 'created before' property dont leave it empty")
                .When(x => x.CreatedBefore != null);

            RuleFor(x => x.CompletionDateBefore)
                .NotEmpty().WithMessage("If you pass 'completion date before' property dont leave it empty")
                .When(x => x.CompletionDateBefore != null);

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Incorrect type of status!")
                .When(x => x.Status != null); ;

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Incorrect type of priority!")
                .When(x => x.Priority != null); ;

            RuleFor(x => x.PageSize)
                .NotEmpty().WithMessage("If you pass 'page size' property dont leave it empty")
                .GreaterThan(0).WithMessage("Page size must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("Page size must be lesser than 101");

            RuleFor(x => x.PageNumber)
                .NotEmpty().WithMessage("If you pass 'page number' property dont leave it empty")
                .GreaterThan(0).WithMessage("Page number must be greater than 0");
        }
    }
}
