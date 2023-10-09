using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validations.Accounts
{
    public class ChangePassowordValidator : AbstractValidator<ChangePasswordDto>
    {

        /*
         *     public string Email { get; set; }
        public int Otp {  get; set; }
        public string NewPassword {  get; set; }
        public string ConfirmPassword {  get; set; }
        */

        public ChangePassowordValidator()
        {


            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters");


            RuleFor(c => c.NewPassword)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .MaximumLength(50).WithMessage("Password must not exceed 50 characters")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one digit")
                .Matches(@"[\W]").WithMessage("Password must contain at least one special character (symbol)");

            RuleFor(c => c.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required")
                .Equal(c => c.NewPassword).WithMessage("Passwords do not match");
        }
    }
}
