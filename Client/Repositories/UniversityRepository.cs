

using API.DTOs.Employees;
using API.Models;
using Client.Contracts;

namespace Client.Repositories
{
    public class UniversityRepository : GeneralRepository<University, Guid>, IUniversityRepository
    {
        public UniversityRepository(string request = "University/") : base(request)
        {

        }
    }
}
