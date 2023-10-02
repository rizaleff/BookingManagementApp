using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _employeeRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _employeeRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        employee.CreatedDate = DateTime.Now;
        employee.ModifiedDate = employee.CreatedDate;

        var result = _employeeRepository.Create(employee);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var employeeByGuid = _employeeRepository.GetByGuid(guid);
        if (employeeByGuid is null)
        {
            return NotFound("ID Not Found");
        }
        var result = _employeeRepository.Delete(employeeByGuid);
        if (!result)
        {
            return BadRequest("Failed to delete data");
        }
        return Ok(result);
    }

    [HttpPut("{guid}")]
    public IActionResult UpdateByGuid(Guid guid, Employee employee)
    {
        var employeeByGuid = _employeeRepository.GetByGuid(guid);
        if (employeeByGuid is null)
        {
            return NotFound("ID Not Found");
        }
        employeeByGuid.Nik = employee.Nik;
        employeeByGuid.FirstName = employee.FirstName;
        employeeByGuid.LastName = employee.LastName;
        employeeByGuid.BirthDate = employee.BirthDate;
        employeeByGuid.Gender = employee.Gender;
        employeeByGuid.HiringDate = employee.HiringDate;
        employeeByGuid.Email = employee.Email;
        employeeByGuid.PhoneNumber = employee.PhoneNumber;
        employeeByGuid.ModifiedDate = DateTime.Now;

        var result = _employeeRepository.Update(employeeByGuid);
        if (!result)
        {
            return BadRequest("Failed to Update Date");

        }
        return Ok(result);
    }

}
