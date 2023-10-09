using API.DTOs.AccountRoles;
using FluentValidation;

namespace API.Utilities.Validations.AccountRoles;
/*
 * Kelas Validator untuk CreateAccountRoleDto
 */
public class CreateAccountRoleValidator : AbstractValidator<CreateAccountRoleDto>
{
    public CreateAccountRoleValidator()
    {
        //Validasi atribut AccountGuid
        RuleFor(a => a.AccountGuid)
            .NotEmpty(); //Atribut tidak boleh kosong atau null

        //Validasi atribut RoleGuid
        RuleFor(a => a.RoleGuid)
            .NotEmpty(); //Atribut tidak boleh kosong atau null
    }
}

