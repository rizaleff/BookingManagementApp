using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;
public class AccountRoleRepository : GeneralRepository<AccountRole>, IAccountRoleRepository
{
    public AccountRoleRepository(BookingManagementDbContext context) : base(context) { }

    public IEnumerable<Guid> GetRolesGuidByAccountGuid(Guid accountGuid)
    {
        return _context.Set<AccountRole>().Where(ar => ar.AccountGuid == accountGuid).Select(ar => ar.RoleGuid);
    }
}
