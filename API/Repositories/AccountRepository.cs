using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;
public class AccountRepository : GeneralRepository<Account>, IAccountRepository
{
    public AccountRepository(BookingManagementDbContext context) : base(context) { }

    public string? GetPasswordByGuid(Guid guid)
    {
        return _context.Accounts.Where(e => e.Guid == guid)
            .Select(e => e.Password).FirstOrDefault();


    }
}
