using API.Models;

namespace API.DTOs.Educations;
public class EducationDto
{
    //Deklarasi atribut yang dibutuhkan sebagai DTO
    public Guid Guid { get; set; }
    public string Major { get; set; }
    public float Gpa { get; set; }
    public Guid UniversityGuid { get; set; }

    /*
     *<summary>Implicit opertor untuk mapping dari EducationDto ke Education secara implisit<summary>
     *<param name="educationDto>Object EducationDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object Education</return>
     */
    public static implicit operator Education(EducationDto educationDto)
    {
        return new Education
        {
            Guid = educationDto.Guid,
            Major = educationDto.Major,
            Gpa = educationDto.Gpa,
            UniversityGuid = educationDto.UniversityGuid,
            ModifiedDate = DateTime.Now
        };
    }

    /*
     *<summary>Explicit opertor untuk mapping dari Education ke EducationDto secara eksplisit<summary>
     *<param name="education>Object Education yang akan di Mapping</param>
     *<return>Hasil mapping berupa object EducationDto</return>
     */
    public static explicit operator EducationDto(Education education)
    {
        return new EducationDto
        {
            Guid = education.Guid,
            Major = education.Major,
            Gpa = education.Gpa,
            UniversityGuid = education.UniversityGuid
        };
    }
}
