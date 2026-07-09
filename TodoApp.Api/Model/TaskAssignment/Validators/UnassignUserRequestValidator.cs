using FluentValidation;
using TodoApp.Api.Model.TaskAssignment.Dto;

namespace TodoApp.Api.Model.TaskAssignment.Validators
{
    public class UnassignUserRequestValidator : AbstractValidator<UnassignUserRequest>
    {
        public UnassignUserRequestValidator() 
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User id must be not empty");
        }
    }
}
