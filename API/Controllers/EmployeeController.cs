using API.Contracts;
using API.DTOs.Employees;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController] //Menandadakan bahwa kelas ini merupakan sebuah controller API
[Route("api/[controller]")] //format route dari tiap endpoint pada controller ini

//Deklarasi kelas BookingController yang merupakan turunan dari kelas ControllerBase
public class EmployeeController : ControllerBase
{
    //sebagai perantara untuk melakukan CRUD melalui contract yang telah dibuat
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data dari semua Employee</summary>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpGet]
    public IActionResult GetAll()
    {
        //Mendapatkan data Employee dan disimpan pada variabel result
        var result = _employeeRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        //mapping setiap item variabel result ke dalam object dari kelas EmployeeDto menggunakan explicit operator
        var data = result.Select(x => (EmployeeDto)x);
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
        var result = _employeeRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }

        //mapping variabel result ke EmployeeDto menggunakan explicit operator
        return Ok((EmployeeDto)result);
    }

    /*
     *<summary>request HTTP POST untuk menambahkan Employee baru</summary>
     *<param name="createEmployeeDto">Data yang akan ditambahkan, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPost]
    public IActionResult Create(CreateEmployeeDto employeeDto)
    {
        //Mapping secara implisit pada createEmployeeDto untuk dijadikan objek Employee
        var result = _employeeRepository.Create(employeeDto);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        //Mapping variabel result ke EmployeeDto menggunakan explicit operator
        return Ok((EmployeeDto)result);
    }

    /*
     *<summary>request HTTP DELETE untuk menambahkan menghapus data berdasarkan Guid</summary>
     *<param name="guid">Guid dari data yang akan dihapus, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpDelete]
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
        return Ok("Data Deleted");
    }

    /*
     *<summary>request HTTP PUT untuk melakukan perubahan data Employee </summary>
     *<param name="employeeDto">Data yang akan dijadikan perubahan, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPut]
    public IActionResult UpdateByGuid(EmployeeDto employeeDto)
    {
        //Mendapatkan data Employee berdasarkan guid
        var employeeByGuid = _employeeRepository.GetByGuid(employeeDto.Guid);
        if (employeeByGuid is null)
        {
            return NotFound("ID Not Found");
        }

        //Menyimpan data dari parameter ke dalam objek toUpdate, serta dilakukan mapping secara implisit
        Employee toUpdate = employeeDto;
        
        toUpdate.CreatedDate = employeeByGuid.CreatedDate;

        //Inisialiasi nilai CreatedDate agar tidak ada perubahan dari data awal
        var result = _employeeRepository.Update(toUpdate);
        if (!result)
        {
            return BadRequest("Failed to Update Date");

        }
        return Ok(result);
    }

}
