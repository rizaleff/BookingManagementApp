using API.Models;

namespace API.DTOs.Roles;
public class RoleDto
{
    //Deklarasi atribut yang dibutuhkan sebagai DTO
    public Guid Guid { get; set; }  
    public string Name { get; set; }

    /*
     *<summary>Implicit opertor untuk mapping dari RoleDto ke Role secara implisit<summary>
     *<param name="roleDto>Object RoleDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object Role</return>
     */
    public static implicit operator Role(RoleDto roleDto)
    {
        return new Role
        {
            Guid = roleDto.Guid,
            Name = roleDto.Name,
            ModifiedDate = DateTime.Now
        };
    }

    /*
     *<summary>Explicit opertor untuk mapping dari Role ke RoleDto secara eksplisit<summary>
     *<param name="role>Object Role yang akan di Mapping</param>
     *<return>Hasil mapping berupa object RoleDto</return>
     */
    public static explicit operator RoleDto(Role role)
    {
        return new RoleDto
        {
            Guid = role.Guid,
            Name = role.Name
        };
    }
}
