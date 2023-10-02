using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AccountRoleController : ControllerBase
{
    private readonly IAccountRoleRepository _accountRoleRepository;

    public AccountRoleController(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _accountRoleRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _accountRoleRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(AccountRole accountRole)
    {
        accountRole.CreatedDate = DateTime.Now;
        accountRole.ModifiedDate = accountRole.CreatedDate;

        var result = _accountRoleRepository.Create(accountRole);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var accountRoleByGuid = _accountRoleRepository.GetByGuid(guid);
        if (accountRoleByGuid is null)
        {
            return NotFound("ID Not Found");
        }
        var result = _accountRoleRepository.Delete(accountRoleByGuid);
        if (!result)
        {
            return BadRequest("Failed to delete data");
        }
        return Ok(result);
    }

    [HttpPut("{guid}")]
    public IActionResult UpdateByGuid(Guid guid, AccountRole accountRole)
    {
        var accountRoleByGuid = _accountRoleRepository.GetByGuid(guid);
        if (accountRoleByGuid is null)
        {
            return NotFound("ID Not Found");
        }
        accountRoleByGuid.AccountGuid = accountRole.AccountGuid;
        accountRoleByGuid.RoleGuid = accountRole.RoleGuid;
        accountRoleByGuid.ModifiedDate = DateTime.Now;
 
        var result = _accountRoleRepository.Update(accountRoleByGuid);
        if (!result)
        {
            return BadRequest("Failed to Update Date");

        }
        return Ok(result);
    }

}
