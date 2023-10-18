using API.Contracts;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;
/*
 * Deklarasi kelas EmployeeRepository yang merupakan kelas turunan dari GeneralRepository dengan parameter <Employee>
 * Kelas ini mengimplementasikan interface IEmployeeRepository
 */
public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(BookingManagementDbContext context) : base(context) { }
    
    /*
     * <summary>Method GetLastNik digunakan untuk mendapatkan nilai NIK terakhir pada tabel Employee</summary>
     * <returns>
     * Kembalian berupa string. Jika objek employee null maka akan mengembalikan empty string
     * Jika objek employee tidak null maka akan menegembalikan nilai berupa NIK dari employee tersebut
     * </returns>
     */
    public string GetLastNik()
    {
        //LINQ untuk mendapatkan Nilai NIK terakhir (terbesar)
        Employee? employee = _context.Employees.OrderByDescending(e => e.Nik).FirstOrDefault();

        return employee?.Nik ?? "";
    }

    public Guid GetGuidByEmail(string email)
    {
        Guid guid = _context.Employees.Where(e => e.Email == email)
            .Select(e => e.Guid).FirstOrDefault();
        
        return guid;

    }

    public Employee GetByEmail(string email)
    {
        Employee? employee = _context.Set<Employee>().FirstOrDefault(e => e.Email == email);
        return employee;
    }

    public string? GetNameByGuid(Guid guid)
    {
        return _context.Set<Employee>().Where(e => e.Guid == guid)
                                .Select(e => e.FirstName + " " + e.LastName)
                                .FirstOrDefault();
    }
}


