using API.Models;

namespace API.Contracts;
public interface IEmployeeRepository : IGeneralRepository<Employee>
{
    //abstract method GetLastNik dengan tipe kembalian string
    string GetLastNik();
    Employee? GetByEmail(string email);
    Guid GetGuidByEmail(string email);

    String GetNameByGuid(Guid guid);
}
