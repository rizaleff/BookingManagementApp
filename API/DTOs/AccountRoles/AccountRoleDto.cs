using API.Models;

namespace API.DTOs.AccountRoles;
public class AccountRoleDto
{
    /*
     * Deklarasi atribut yang dibutuhkan
     */
    public Guid Guid { get; set; }
    public Guid AccountGuid { get; set; }
    public Guid RoleGuid { get; set; }

    /*
     *<summary>Implicit opertor untuk mapping dari AccountRoleDto ke AccountRole secara implisit<summary>
     *<param name="accountRoleDto>Object AccountRoleDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object AccountRole</return>
     */
    public static implicit operator AccountRole(AccountRoleDto accountRoleDto)
    {
        return new AccountRole
        {
            Guid = accountRoleDto.Guid,
            AccountGuid = accountRoleDto.AccountGuid,
            RoleGuid = accountRoleDto.RoleGuid,
            ModifiedDate = DateTime.Now
        };
    }

    /*
     *<summary>Explicit opertor untuk mapping dari AccountRole ke AccountRoleDto secara eksplisit<summary>
     *<param name="accountRole>Object AccountRole yang akan di Mapping</param>
     *<return>Hasil mapping berupa object AccountRoleDto</return>
     */
    public static explicit operator AccountRoleDto(AccountRole accountRole)
    {
        return new AccountRoleDto
        {
            Guid = accountRole.Guid,
            AccountGuid = accountRole.AccountGuid,
            RoleGuid = accountRole.RoleGuid
        };
    }

}
