using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;

    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _accountRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _accountRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(Account account)
    {
        account.CreatedDate = DateTime.Now;
        account.ModifiedDate = account.CreatedDate;

        var result = _accountRepository.Create(account);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var accountByGuid = _accountRepository.GetByGuid(guid);
        if (accountByGuid is null)
        {
            return NotFound("ID Not Found");
        }
        var result = _accountRepository.Delete(accountByGuid);
        if (!result)
        {
            return BadRequest("Failed to delete data");
        }
        return Ok(result);
    }

    [HttpPut("{guid}")]
    public IActionResult UpdateByGuid(Guid guid, Account account)
    {
        var accountByGuid = _accountRepository.GetByGuid(guid);
        if (accountByGuid is null)
        {
            return NotFound("ID Not Found");
        }
        accountByGuid.Password = account.Password;
        accountByGuid.Otp = account.Otp;
        accountByGuid.IsUsed = account.IsUsed;
        accountByGuid.ExpiredTime = account.ExpiredTime;
        accountByGuid.ModifiedDate = DateTime.Now;

        var result = _accountRepository.Update(accountByGuid);
        if (!result)
        {
            return BadRequest("Failed to Update Date");

        }
        return Ok(result);
    }
}
