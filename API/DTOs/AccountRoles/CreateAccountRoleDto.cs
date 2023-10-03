using API.Models;

namespace API.DTOs.AccountRoles;
public class CreateAccountRoleDto
{
    //Deklarasi atribut yang dibutuhkan sebagai DTO
    public Guid AccountGuid { get; set; }
    public Guid RoleGuid { get; set; }

    /*
     *<summary>Implicit opertor untuk mapping dari CreateAccountRoleDto ke AccountRole secara implisit<summary>
     *<param name="createAccountRoleDto>Object CreateAccountRoleDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object AccountRole</return>
     */
    public static implicit operator AccountRole(CreateAccountRoleDto createAccountRoleDto)
    {
        return new AccountRole
        {
            AccountGuid = createAccountRoleDto.AccountGuid,
            RoleGuid = createAccountRoleDto.RoleGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}
