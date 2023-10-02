using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EducationController : ControllerBase
{
    private readonly IEducationRepository _educationRepository;

    public EducationController(IEducationRepository educationRepository)
    {
        _educationRepository = educationRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _educationRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _educationRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(Education education)
    {
        education.CreatedDate = DateTime.Now;
        education.ModifiedDate = education.CreatedDate;

        var result = _educationRepository.Create(education);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var educationById = _educationRepository.GetByGuid(guid);
        if (educationById is null)
        {
            return NotFound("ID Not Found");
        }
        var result = _educationRepository.Delete(educationById);
        if (!result)
        {
            return BadRequest("Failed to delete data");
        }
        return Ok(result);
    }

    [HttpPut("{guid}")]
    public IActionResult Update(Guid guid, Education education)
    {
        var educationById = _educationRepository.GetByGuid(guid);
        if (educationById is null)
        {
            return NotFound("ID Not Found");
        }
        educationById.Major = education.Major;
        educationById.Degree = education.Degree;
        educationById.Gpa = education.Gpa;
        educationById.UniversityGuid = education.UniversityGuid;


        var result = _educationRepository.Update(educationById);
        if (!result)
        {
            return BadRequest("Failed to Update Date");

        }
        return Ok(result);
    }
}
