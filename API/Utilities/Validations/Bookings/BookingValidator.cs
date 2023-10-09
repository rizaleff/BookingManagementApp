using API.DTOs.Bookings;
using FluentValidation;

namespace API.Utilities.Validations.Bookings;
/*
 * Kelas Validator untuk BookingDto
 */
public class BookingValidator : AbstractValidator<BookingDto>
{
    public BookingValidator()
    {
        //Validasi atribut Guid
        RuleFor(b => b.Guid)
            .NotEmpty(); //Atribut tidak boleh kosong atau null

        //Validasi atribut StartDate
        RuleFor(b => b.StartDate)
            .NotEmpty(); //Atribut tidak boleh kosong atau null

        //Validasi atribut EndDate
        RuleFor(b => b.EndDate) 
            .NotEmpty(); //Atribut tidak boleh kosong atau null
        
        //Validasi atribut Status
        RuleFor(b => b.Status)
            .NotNull() //Atribut tidak boleh null
            .IsInEnum(); //Nilai atribut harus di range Index Enum

        //Validasi atribut Remarks
        RuleFor(b => b.Remarks)
            .NotEmpty() //Atribut tidak boleh kosong atau null
            .MinimumLength(3) //Nilai atribut harus memiliki setidaknya 3 karakter
            .MaximumLength(100); //Nilai atribut maksimal memiliki setidaknya 100 karakter

        //Validasi atribut RoomGuid
        RuleFor(b => b.RoomGuid)
            .NotEmpty(); //Atribut tidak boleh kosong atau null

        //Validasi atribut EmployeeGuid
        RuleFor(b => b.EmployeeGuid)
            .NotEmpty(); //Atribut tidak boleh kosong atau null
    }
}
