using API.Models;

namespace API.Contracts;
public interface IAccountRoleRepository : IGeneralRepository<AccountRole>
{
    IEnumerable<Guid> GetRolesGuidByAccountGuid(Guid accountGuid);
}
