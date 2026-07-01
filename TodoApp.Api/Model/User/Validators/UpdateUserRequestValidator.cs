using FluentValidation;
using TodoApp.Api.Model.User.Dto;

namespace TodoApp.Api.Model.TodoTasks.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is obligatory!")
                .MaximumLength(64).WithMessage("First name should have less than 65 symbols!")
                .When(x => x.FirstName != null);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name should be filled!")
                .MaximumLength(64).WithMessage("Last name should contain less than 65 symbols!")
                .When(x => x.LastName != null);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is mandatory!")
                .EmailAddress().WithMessage("Check if email address is valid")
                .When(x => x.Email != null);

            RuleFor(x => x.Position)
                .NotEmpty().WithMessage("Position is obligatory!")
                .MaximumLength(128).WithMessage("Position should have less than 129 symbols!")
                .When(x => x.Position != null);
        }
    }
}
