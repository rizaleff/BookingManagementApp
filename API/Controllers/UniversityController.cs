using API.Contracts;
using API.DTOs;
using API.DTOs.Universities;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController] //Menandadakan bahwa kelas ini merupakan sebuah controller API
[Route("api/[controller]")] //format route dari tiap endpoint pada controller ini

//Deklarasi kelas UniversityController yang merupakan turunan dari kelas ControllerBase
public class UniversityController : ControllerBase
{
    //sebagai perantara untuk melakukan CRUD melalui contract yang telah dibuat
    private readonly IUniversityRepository _universityRepository;

    public UniversityController(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    /*
    *<summary>request HTTP GET untuk mendpatkan data dari semua University</summary>
    *<returns>return value berupa status HTTP response status codes</returns> 
    */
    [HttpGet]
    public IActionResult GetAll()
    {
        //Mendapatkan data University dan disimpan pada variabel result
        var result = _universityRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }
        //mapping setiap item variabel result ke dalam object dari kelas UniversityDto menggunakan explicit operator
        var data = result.Select(x => (UniversityDto)x);


        return Ok(data);
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data berdasarkan Guid yang dimasukkan pada parameter</summary>
     *<param name="guid">guid yang didapatkan dari path</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _universityRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        //mapping variabel result ke UniversityDto menggunakan explicit operator
        return Ok((UniversityDto)result);
    }

    /*
     *<summary>request HTTP POST untuk menambahkan University baru</summary>
     *<param name="createUniversityDto">Data yang akan ditambahkan, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPost]
    public IActionResult Create(CreateUniversityDto universityDto)
    {
        //Mapping secara implisit pada createUniversityDto untuk dijadikan objek University
        var result = _universityRepository.Create(universityDto);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        //Mapping variabel result ke UniversityDto menggunakan explicit operator
        return Ok((UniversityDto)result);
    }

    /*
     *<summary>request HTTP DELETE untuk menambahkan menghapus data berdasarkan Guid</summary>
     *<param name="guid">Guid dari data yang akan dihapus, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpDelete]
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
        return Ok("Data Deleted");
    }

    /*
     *<summary>request HTTP PUT untuk melakukan perubahan data University </summary>
     *<param name="universityDto">Data yang akan dijadikan perubahan, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPut]
    public IActionResult UpdateByGuid(UniversityDto universityDto)
    {
        //Mendapatkan data University berdasarkan guid
        var universityById = _universityRepository.GetByGuid(universityDto.Guid);
        if (universityById is null)
        {
            return NotFound("ID Not Found");
        }
        //Menyimpan data dari parameter ke dalam objek toUpdate, serta dilakukan mapping secara implisit
        University toUpdate = universityDto;
        
        //Inisialiasi nilai CreatedDate agar tidak ada perubahan dari data awal
        toUpdate.CreatedDate = universityById.CreatedDate;

        //Melakukan Update dengan parameter toUpdate
        var result = _universityRepository.Update(toUpdate);
        if (!result)
        {
            return BadRequest("Failed to Update Date");

        }
        return Ok("Data Updated");
    }
}
