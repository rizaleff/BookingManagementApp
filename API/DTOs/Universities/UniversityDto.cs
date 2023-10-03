using API.Models;

namespace API.DTOs.Universities;
public class UniversityDto
{
    //Deklarasi atribut yang dibutuhkan sebagai DTO
    public Guid Guid { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }

    /*
     *<summary>Implicit opertor untuk mapping dari UniversityDto ke University secara implisit<summary>
     *<param name="university>Object UniversityDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object University</return>
     */
    public static implicit operator University(UniversityDto universityDto)
    {
        return new University
        {
            Guid = universityDto.Guid,
            Code = universityDto.Code,
            Name = universityDto.Name,
            ModifiedDate = DateTime.Now
        };
    }

    /*
     *<summary>explicit operator untuk mapping dari University ke UniversityDto secara eksplisit<summary>
     *<param name="university>Object University yang akan di Mapping</param>
     *<return>Hasil mapping berupa object UniversityDto</return>
     */
    public static explicit operator UniversityDto(University university)
    {
        return new UniversityDto
        {
            Guid = university.Guid,
            Code = university.Code,
            Name = university.Name
        };
    }
}
