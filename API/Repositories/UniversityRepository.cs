using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;
public class UniversityRepository : GeneralRepository<University>, IUniversityRepository
{
    public UniversityRepository(BookingManagementDbContext context) : base(context) { }

    public Guid UniversityGuidByName(string universityName)
    {
        Guid guid = _context.Universities.Where(e => e.Name == universityName)
            .Select(e => e.Guid).FirstOrDefault();
        return guid;
    }

}
