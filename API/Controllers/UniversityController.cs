using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UniversityController : ControllerBase
{
    private readonly IUniversityRepository _universityRepository;

    public UniversityController(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _universityRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _universityRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(University university)
    {
        university.CreatedDate = DateTime.Now;
        university.ModifiedDate = university.CreatedDate;

        var result = _universityRepository.Create(university);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var universityById = _universityRepository.GetByGuid(guid);
        if (universityById is null)
        {
            return NotFound("ID Not Found");
        }
        var result = _universityRepository.Delete(universityById);
        if (!result)
        {
            return BadRequest("Failed to delete data");
        }
        return Ok(result);
    }

    [HttpPut("{guid}")]
    public IActionResult UpdateByGuid(Guid guid, University university)
    {
        var universityById = _universityRepository.GetByGuid(guid);
        if (universityById is null)
        {
            return NotFound("ID Not Found");
        }
        universityById.Code = university.Code;
        universityById.Name = university.Name;
        universityById.ModifiedDate = DateTime.Now;
        var result = _universityRepository.Update(universityById);
        if (!result)
        {
            return BadRequest("Failed to Update Date");

        }
        return Ok(result);
    }
}
