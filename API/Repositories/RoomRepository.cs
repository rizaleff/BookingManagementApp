using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;
public class RoomRepository : GeneralRepository<Room>, IRoomRepository
{
    public RoomRepository(BookingManagementDbContext context) : base(context) { }

    public string GetNameByGuid(Guid guid)
    {
        return _context.Set<Room>().FirstOrDefault(r => r.Guid == guid).Name;
    }
}