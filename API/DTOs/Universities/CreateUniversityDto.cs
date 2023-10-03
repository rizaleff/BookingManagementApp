using API.Models;

namespace API.DTOs;
public class CreateUniversityDto
{
    //Deklarasi atribut yang dibutuhkan sebagai DTO
    public string Code { get; set; }
    public string Name { get; set; }

    /*
     *<summary>Implicit opertor untuk mapping dari CreateUniversityDto ke University secara implisit<summary>
     *<param name="createUniversityDto>Object CreateUniversityDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object University</return>
     */
    public static implicit operator University(CreateUniversityDto createUniversityDto)
    {
        return new University
        {
            Code = createUniversityDto.Code,
            Name = createUniversityDto.Name,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}
