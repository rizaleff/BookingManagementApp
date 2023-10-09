using API.DTOs.AccountRoles;
using FluentValidation;

namespace API.Utilities.Validations.AccountRoles;
/*
 * Kelas Validator untuk AccountRoleDto
 */
public class AccountRolesValidator : AbstractValidator<AccountRoleDto>
{
    /*
     * Constructor yang berisi validasi
     */
    public AccountRolesValidator()
    {
        //Validasi pada atribut Guid
        RuleFor(a => a.Guid)
            .NotEmpty(); //Atribut tidak boleh kosong atau null

        RuleFor(a => a.AccountGuid)
            .NotEmpty(); //Atribut tidak boleh kosong atau null

        RuleFor(a => a.RoleGuid)
                .NotEmpty(); //Atribut tidak boleh kosong atau null
    }
}


