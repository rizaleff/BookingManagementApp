using API.Models;

namespace API.Contracts;
public interface IUniversityRepository : IGeneralRepository<University>
{
    Guid UniversityGuidByName(string universityName);
}
