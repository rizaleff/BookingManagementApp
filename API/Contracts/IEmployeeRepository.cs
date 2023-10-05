using API.Models;

namespace API.Contracts;
public interface IEmployeeRepository : IGeneralRepository<Employee>
{
    //abstract method GetLastNik dengan tipe kembalian string
    string GetLastNik();
    Guid GetGuidByEmail(string email);
}
