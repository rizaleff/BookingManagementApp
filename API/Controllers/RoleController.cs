using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;

    public RoleController(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _roleRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _roleRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(Role role)
    {
        role.CreatedDate = DateTime.Now;
        role.ModifiedDate = role.CreatedDate;

        var result = _roleRepository.Create(role);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var roleByGuid = _roleRepository.GetByGuid(guid);
        if (roleByGuid is null)
        {
            return NotFound("ID Not Found");
        }
        var result = _roleRepository.Delete(roleByGuid);
        if (!result)
        {
            return BadRequest("Failed to delete data");
        }
        return Ok(result);
    }

    [HttpPut("{guid}")]
    public IActionResult UpdateByGuid(Guid guid, Role role)
    {
        var roleByGuid = _roleRepository.GetByGuid(guid);
        if (roleByGuid is null)
        {
            return NotFound("ID Not Found");
        }
        roleByGuid.Name = role.Name;

        roleByGuid.ModifiedDate = DateTime.Now;

        var result = _roleRepository.Update(roleByGuid);
        if (!result)
        {
            return BadRequest("Failed to Update Date");

        }
        return Ok(result);
    }


}
