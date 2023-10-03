using API.Models;

namespace API.DTOs.Roles;
public class CreateRoleDto
{
    //Deklarasi atribut yang dibutuhkan sebagai DTO
    public string Name {  get; set; }

    /*
     *<summary>Implicit opertor untuk mapping dari CreateRoleDto ke Role secara implisit<summary>
     *<param name="createRoleDto>Object CreateRoleDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object Role</return>
     */
    public static implicit operator Role(CreateRoleDto createRoleDto)
    {
        return new Role
        {
            Name = createRoleDto.Name,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}
