using API.Models;

namespace API.Contracts;
public interface IRoomRepository : IGeneralRepository<Room>
{
    String GetNameByGuid(Guid guid);
}
