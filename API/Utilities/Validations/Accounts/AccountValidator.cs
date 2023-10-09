using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validations.Accounts;
/*
 * Kelas Validator untuk AccountDto
 */
public class AccountValidator : AbstractValidator<AccountDto>
{
    public AccountValidator()
    {
        //Validasi atribut Guid
        RuleFor(a => a.Guid)
            .NotEmpty(); //Atribut tidak boleh kosong

        //Validasi atribut password
        RuleFor(a => a.Password)
            .NotEmpty() //atribut tidak boleh kosong
            .MinimumLength(8).WithMessage("Password minimal terdiri dari 8 karakter")
            .MaximumLength(23).WithMessage("Password maksimal terdiri dari 23 karakter")
            .Matches("[A-Z]").WithMessage("Password harus mengandung setidaknya satu huruf kapital")
            .Matches("[a-z]").WithMessage("Password harus mengandung setidaknya satu huruf kecil")
            .Matches("[0-9]").WithMessage("Password harus mengandung setidaknya satu angka");

        //Validasi atribut otp
        RuleFor(a => a.Otp)
            .NotNull().WithMessage("OTP Tidak Boleh Kosong");

        //Validasi atribut IsUsed
        RuleFor(a => a.IsUsed)
            .NotNull().WithMessage("IsUsed Tidak Boleh Kosong");

       
    }
}
