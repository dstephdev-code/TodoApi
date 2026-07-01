using FluentValidation;
using TodoApp.Api.Model.TodoTasks.Dto;

namespace TodoApp.Api.Model.TodoTasks.Validators
{
    public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
    {
        public CreateTaskRequestValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Task name is obligatory!")
                .MaximumLength(64).WithMessage("Task name should have less than 65 symbols!");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Task description should be filled!")
                .MaximumLength(256).WithMessage("Task description should contain less than 257 symbols!");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("Complition date is mandatory!")
                .GreaterThan(DateTimeOffset.UtcNow).WithMessage("Complition date must not be in the past!");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Incorrect type of priority!");
        }
    }
}
