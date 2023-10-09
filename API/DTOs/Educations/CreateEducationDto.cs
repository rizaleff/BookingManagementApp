using API.Models;

namespace API.DTOs.Educations;
public class CreateEducationDto
{
    //Deklarasi atribut yang dibutuhkan sebagai DTO
    public Guid Guid {  get; set; }
    public string Major { get; set; }
    public string Degree { get; set; }

    public float Gpa { get; set; }

    public Guid UniversityGuid {  get; set; }

    /*
     *<summary>Implicit opertor untuk mapping dari CreateEducationDto ke Education secara implisit<summary>
     *<param name="createEducationDto>Object CreateEducationDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object Education</return>
     */
    public static implicit operator Education(CreateEducationDto createEducationDto)
    {
        return new Education
        {
            Guid = createEducationDto.Guid,
            Major = createEducationDto.Major,
            Degree = createEducationDto.Degree,
            Gpa = createEducationDto.Gpa,
            UniversityGuid = createEducationDto.UniversityGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

}
