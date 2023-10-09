using API.DTOs;
using FluentValidation;

namespace API.Utilities.Validations.Universities;
/*
 * Kelas Validator untuk CreateUniversityDto
 */
public class CreateUniversityValidator : AbstractValidator<CreateUniversityDto>
{
    public CreateUniversityValidator() 
    {
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


