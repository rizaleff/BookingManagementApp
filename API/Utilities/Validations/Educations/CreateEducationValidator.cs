using API.DTOs.Educations;
using FluentValidation;

namespace API.Utilities.Validations.Educations;
/*
 * Kelas Validator untuk CreateEducationDto
 */
public class CreateEducationValidator : AbstractValidator<CreateEducationDto>
{
    public CreateEducationValidator()
    {
        //Validasi atribut Major
        RuleFor(e => e.Major)
           .NotEmpty(); //Atribut tidak boleh kosong atau null

        //Validasi atribut Gpa
        RuleFor(e => e.Gpa)
            .NotNull() //Atribut tidak boleh bernilai null
            .LessThanOrEqualTo(4) //Nilai atribut harus kurang dari atau sama dengan 4
            .GreaterThanOrEqualTo(0); //nilai atribut harus lebih dari atau sama dengan 0
      

        //Validasi atribut UniversityGuid
        RuleFor(e => e.UniversityGuid)
            .NotEmpty(); //Atribut tidak boleh kosong atau null
    }
}

