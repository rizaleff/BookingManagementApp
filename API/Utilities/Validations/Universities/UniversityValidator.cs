using API.DTOs.Universities;
using FluentValidation;

namespace API.Utilities.Validations.Universities;
/*
 * Kelas Validator untuk UniversityDto
 */
public class UniversityValidator : AbstractValidator<UniversityDto>
{
    public UniversityValidator()
    {
        //Validasi atribut Guid
        RuleFor(u => u.Guid)
            .NotEmpty(); //Atribut tidak boleh bernilai kosong atau null

        //Validasi atribut Code
        RuleFor(u => u.Code)
            .NotEmpty() //Atribut tidak boleh bernilai kosong atau null
            .MinimumLength(2) //Atribut harus memiliki panjang minimal 2 Karakter
            .MaximumLength(10); //Atribut harus memiliki panjang maksimal 10 karakter

        //Validasi atribut Name
        RuleFor(u => u.Name)
            .NotEmpty() //Atribut tidak boleh bernilai kosong atau null
            .MinimumLength(2) //Atribut harus memiliki panjang minimal 2 Karakter
            .MaximumLength(100); //Atribut harus memiliki panjang maksimal 100 karakter
    }
}



