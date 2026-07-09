using FluentValidation;
using TodoApp.Api.Model.TodoTasks.Dto;

namespace TodoApp.Api.Model.TodoTasks.Validators
{
    public class UpdateTaskRequestValidator : AbstractValidator<PatchTaskRequest>
    {
        public UpdateTaskRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Task name is obligatory!")
                .MaximumLength(64).WithMessage("Task name should have less than 65 symbols!")
                .When(x => x.Name != null);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Task description should be filled!")
                .MaximumLength(1024).WithMessage("Task description should contain less than 1025 symbols!")
                .When(x => x.Description != null);

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("Complition date is mandatory!")
                .GreaterThan(DateTimeOffset.UtcNow).WithMessage("Complition date must not be in the past!")
                .When(x => x.DueDate != null);

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Incorrect type of priority!")
                .When(x => x.Priority != null);
        }
    }
}
