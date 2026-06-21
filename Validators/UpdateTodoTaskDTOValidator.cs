using FluentValidation;
using TodoApi.Model;

namespace TodoApi.Validators
{
    public class UpdateTodoTaskDTOValidator : AbstractValidator<TodoTaskUpdateDTO>
    {
        public UpdateTodoTaskDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Task name is obligatory!")
                .MaximumLength(64).WithMessage("Task name should have less than 65 symbols!")
                .When(x => x.Name != null);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Task description should be filled!")
                .MaximumLength(256).WithMessage("Task description should contain less than 257 symbols!")
                .When(x => x.Description != null);

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("Complition date is mandatory!")
                .GreaterThan(DateTimeOffset.UtcNow).WithMessage("Complition date must not be in the past!")
                .When(x => x.DueDate != null);
        }
    }
}
