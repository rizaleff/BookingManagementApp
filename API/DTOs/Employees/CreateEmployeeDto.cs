using API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs.Employees;
public class CreateEmployeeDto
{
    //Deklarasi atribut yang dibutuhkan sebagai DTO
    public string Nik { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public int Gender { get; set; }
    public DateTime HiringDate { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    /*
     *<summary>Implicit opertor untuk mapping dari CreateEmployeeDto ke Employee secara implisit<summary>
     *<param name="createEmployeeDto>Object CreateEmployeeDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object Employee</return>
     */
    public static implicit operator Employee(CreateEmployeeDto createEmployeeDto)
    {
        return new Employee
        {
            Nik = createEmployeeDto.Nik,
            FirstName = createEmployeeDto.FirstName,
            LastName = createEmployeeDto.LastName,
            BirthDate = createEmployeeDto.BirthDate,
            Gender = createEmployeeDto.Gender,
            HiringDate = createEmployeeDto.HiringDate,
            Email = createEmployeeDto.Email,
            PhoneNumber = createEmployeeDto.PhoneNumber,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}