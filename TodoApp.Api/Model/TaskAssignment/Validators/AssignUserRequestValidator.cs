using FluentValidation;
using TodoApp.Api.Model.TaskAssignment.Dto;

namespace TodoApp.Api.Model.TaskAssignment.Validators
{
    public class AssignUserRequestValidator : AbstractValidator<AssignUserRequest>
    {
        public AssignUserRequestValidator() 
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User id must be not empty");

            RuleFor(x => x.AssignedByUserId)
                .NotEmpty().WithMessage("Assigned by user id must be not empty");

            RuleFor(x => x.Comment)
                .MaximumLength(1024).WithMessage("Comment length must be less than 1025 symbols");
        }
    }
}
