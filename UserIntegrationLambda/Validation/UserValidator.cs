using Common.Constants;
using Common.Entities;
using FluentValidation;

namespace UserIntegrationLambda.Validation
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Uuid.ToString()).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Uuid should not be empty")
                .Matches(ValidationConstants.GuidRegexPattern).WithMessage("Uuid should of guid type");
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name should not be empty");

        }
    }
}
