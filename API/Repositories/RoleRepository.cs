using API.Contracts;
using API.Data;
using API.Models;
using API.Repositories;

namespace API.Repositorie;
public class RoleRepository : GeneralRepository<Role>, IRoleRepository
{
    public RoleRepository(BookingManagementDbContext context) : base(context) { }
}