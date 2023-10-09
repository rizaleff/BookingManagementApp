using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace API.Contracts;

public interface ITokenHandler
{
    public string Generate(IEnumerable<Claim> claims);


}
