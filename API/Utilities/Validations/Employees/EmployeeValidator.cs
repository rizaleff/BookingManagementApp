using API.DTOs.Employees;
using FluentValidation;

namespace API.Utilities.Validations.Employees;
/*
 * Kelas Validator untuk EmployeeDto
 */
public class EmployeeValidator : AbstractValidator<EmployeeDto>
{
    public EmployeeValidator()
    {
        //Validasi atribut FIrstName
        RuleFor(e => e.FirstName)
            .NotEmpty().WithMessage("First name tidak boleh kosong")
            .MinimumLength(1).WithMessage("First name setidaknya terdiri dari 1 karakter")
            .MaximumLength(100).WithMessage("First name tidak boleh melebihi 100 karakter");

        //Validasi atribut LastName
        RuleFor(e => e.LastName)
            .MinimumLength(0).WithMessage("Last name setidaknya terdiri dari 1 karakter")
            .MaximumLength(100).WithMessage("Last name tidak boleh melebihi 100 karakter");

        //Validasi atribut BirthDate
        RuleFor(e => e.BirthDate)
            .NotEmpty().WithMessage("BirthDate tidak boleh kosong")
            .LessThanOrEqualTo(DateTime.Now.AddYears(-18)).WithMessage("Minimal harus berusia 18 Tahun");

        //Validasi atribut Gender
        RuleFor(e => e.Gender)
            .NotNull().WithMessage("Gender tidak boleh kosong")
            .IsInEnum();

        //Validasi atribut Hiring Date
        RuleFor(e => e.HiringDate)
            .NotEmpty().WithMessage("Hiring Date Tidak boleh kosong");

        //Validasi atribut Email
        RuleFor(e => e.Email)
            .NotEmpty() ////Atribut tidak boleh kosong atau null
            .MinimumLength(10) //Atribut harus memiliki panjang minimal 10 Karakter
            .MaximumLength(100) //Atribut harus memiliki panjang maksimal 100 karakter
            .EmailAddress(); //Atribut harus berformat alamat email

        //Validasi atribut PhoneNumber
        RuleFor(e => e.PhoneNumber)
            .NotEmpty()
            .MinimumLength(10) //Atribut harus memiliki panjang minimal 10 Karakter
            .MaximumLength(20) //Atribut harus memiliki panjang maksimal 20 karakter
            .Matches("^[0-9]$"); ///Atribut hanya boleh berupa angka
    }
}
