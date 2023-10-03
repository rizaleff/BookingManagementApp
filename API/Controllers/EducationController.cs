using API.Contracts;
using API.DTOs.Educations;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController] //Menandadakan bahwa kelas ini merupakan sebuah controller API
[Route("api/[controller]")] //format route dari tiap endpoint pada controller ini

//Deklarasi kelas EducationController yang merupakan turunan dari kelas ControllerBase
public class EducationController : ControllerBase
{
    //sebagai perantara untuk melakukan CRUD melalui contract yang telah dibuat
    private readonly IEducationRepository _educationRepository;

    public EducationController(IEducationRepository educationRepository)
    {
        _educationRepository = educationRepository;
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data dari semua Education</summary>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpGet]
    public IActionResult GetAll()
    {
        //Mendapatkan data Education dan disimpan pada variabel result
        var result = _educationRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        //mapping setiap item variabel result ke dalam object dari kelas EducationDto menggunakan explicit operator
        var data = result.Select(x => (EducationDto) x);

        return Ok(data);
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data berdasarkan Guid yang dimasukkan pada parameter</summary>
     *<param name="guid">guid yang didapatkan dari path<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _educationRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }

        //mapping variabel result ke EducationDto menggunakan explicit operator
        return Ok((EducationDto)result);
    }

    /*
     *<summary>request HTTP POST untuk menambahkan Education baru</summary>
     *<param name="createEducationDto">Data yang akan ditambahkan, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPost]
    public IActionResult Create(CreateEducationDto createEducationDto)
    {
        //Mapping secara implisit pada createEducationDto untuk dijadikan objek Education
        var result = _educationRepository.Create(createEducationDto);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        //Mapping variabel result ke EducationDto menggunakan explicit operator
        return Ok((EducationDto)result);
    }

    /*
     *<summary>request HTTP DELETE untuk menambahkan menghapus data berdasarkan Guid</summary>
     *<param name="guid">Guid dari data yang akan dihapus, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpDelete]
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

    /*
     *<summary>request HTTP PUT untuk melakukan perubahan data Education </summary>
     *<param name="educationDto">Data yang akan dijadikan perubahan, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPut]
    public IActionResult Update(EducationDto educationDto)
    {
        //Mendapatkan data Education berdasarkan guid
        var educationById = _educationRepository.GetByGuid(educationDto.Guid);
        if (educationById is null)
        {
            return NotFound("ID Not Found");
        }
        
        //Menyimpan data dari parameter ke dalam objek toUpdate, serta dilakukan mapping secara implisit
        Education toUpdate = educationDto;
        
        //Inisialiasi nilai CreatedDate agar tidak ada perubahan dari data awal
        toUpdate.ModifiedDate = DateTime.Now;

        var result = _educationRepository.Update(educationById);
        if (!result)
        {
            return BadRequest("Failed to Update Date");

        }
        return Ok(result);
    }
}
