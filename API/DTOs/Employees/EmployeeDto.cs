using API.Models;

namespace API.DTOs.Employees;
public class EmployeeDto
{
    //Deklarasi atribut yang dibutuhkan sebagai DTO
    public Guid Guid { get; set; }
    public string Nik { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public int Gender { get; set; }
    public DateTime HiringDate { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    /*
     *<summary>Implicit opertor untuk mapping dari EmployeeDto ke Employee secara implisit<summary>
     *<param name="employeeDto>Object EmployeeDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object Employee</return>
     */
    public static implicit operator Employee(EmployeeDto employeeDto)
    {
        return new Employee
        {
            Guid = employeeDto.Guid,
            Nik = employeeDto.Nik,
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            BirthDate = employeeDto.BirthDate,
            Gender = employeeDto.Gender,
            HiringDate = employeeDto.HiringDate,
            Email = employeeDto.Email,
            PhoneNumber = employeeDto.PhoneNumber,
            ModifiedDate = DateTime.Now
        };
    }

    /*
     *<summary>Explicit opertor untuk mapping dari Employee ke EmployeeDto secara eksplisit<summary>
     *<param name="employee>Object Employee yang akan di Mapping</param>
     *<return>Hasil mapping berupa object EmployeeDto</return>
     */
    public static explicit operator EmployeeDto(Employee employee)
    {
        return new EmployeeDto
        {
            Guid = employee.Guid,
            Nik = employee.Nik,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber
        };
    }


}
