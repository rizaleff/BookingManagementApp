using API.DTOs.Rooms;
using FluentValidation;

namespace API.Utilities.Validations.Rooms;
/*
 * Kelas Validator untuk RoomDto
 */
public class RoomValidator : AbstractValidator<RoomDto>
{
    public RoomValidator()
    {
        //Validasi atribut Guid
        RuleFor(r => r.Guid)
            .NotEmpty(); //Atribut tidak boleh bernilai kosong atau null

        //Validasi atribut Name
        RuleFor(r => r.Name)
            .NotEmpty() //Atribut tidak boleh kosong atau null
            .MinimumLength(2) //Atribut harus memiliki panjang minimal 2 Karakter
            .MaximumLength(100); //Atribut harus memiliki panjang maksimal 100 karakter

        //Validasi atribut Floor
        RuleFor(r => r.Floor)
            .NotNull(); //Atribut tidak boleh bernilai null

        //Validasi atribut Capacity
        RuleFor(r => r.Capacity)
            .NotNull(); //Atribut tidak boleh bernilai null
    }
}


