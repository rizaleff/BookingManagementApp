using API.DTOs.Roles;
using FluentValidation;

namespace API.Utilities.Validations.Roles;
/*
 * Kelas Validator untuk RoleDto
 */
public class RoleValidator : AbstractValidator<RoleDto>
{
    public RoleValidator()
    {
        //Validasi atribut Guid
        RuleFor(r => r.Guid)
            .NotEmpty(); //Atribut tidak boleh kosong atau null

        //Validasi atribut Name
        RuleFor(r => r.Name)
            .NotEmpty() //Atribut tidak boleh kosong atau null
            .MinimumLength(2) //Atribut harus memiliki panjang minimal 2 Karakter
            .MaximumLength(20); //Atribut harus memiliki panjang minimal 20 Karakter
    }
}

