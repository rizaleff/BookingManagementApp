using API.DTOs.Employees;
using FluentValidation;

namespace API.Utilities.Validations.Employees;
public class RegisterEmployeeValidator : AbstractValidator<RegisterEmployeeDto>
{
    public RegisterEmployeeValidator() 
    {


        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Password is required");

        RuleFor(r => r.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm Password is required")
            .Equal(r => r.Password).WithMessage("Passwords do not match");
    }

}
