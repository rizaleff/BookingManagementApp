using API.Contracts;
using API.DTOs.AccountRoles;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController] //Menandadakan bahwa kelas ini merupakan sebuah controller API
[Route("api/[controller]")] //format route dari tiap endpoint pada controller ini

//Deklarasi kelas AccountRoleController yang merupakan turunan dari kelas ControllerBase
public class AccountRoleController : ControllerBase
{
    //sebagai perantara untuk melakukan CRUD melalui contract yang telah dibuat
    private readonly IAccountRoleRepository _accountRoleRepository;  

    public AccountRoleController(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data dari semua AcccountRole</summary>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpGet]
    public IActionResult GetAll()
    {
        //Mendapatkan data AcccountRole dan disimpan pada variabel result
        var result = _accountRoleRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        //mapping setiap item variabel result ke dalam object dari kelas AccountRoleDto menggunakan explicit operator
        var data = result.Select(x => (AccountRoleDto)x);
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
        var result = _accountRoleRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        //mapping variabel result ke AccountRoleDto menggunakan explicit operator
        return Ok((AccountRoleDto)result);
    }

    /*
     *<summary>request HTTP POST untuk menambahkan AcccountRole baru</summary>
     *<param name="createAccountRoleDto">Data yang akan ditambahkan, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPost]
    public IActionResult Create(CreateAccountRoleDto createAccountRoleDto)
    {
        //Mapping secara implisit pada createAccountleDto untuk dijadikan objek AcccountRole
        var result = _accountRoleRepository.Create(createAccountRoleDto);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }
        //Mapping variabel result ke AccountRoleDto menggunakan explicit operator
        return Ok((AccountRoleDto)result);
    }

    /*
     *<summary>request HTTP DELETE untuk menambahkan menghapus data berdasarkan Guid</summary>
     *<param name="guid">Guid dari data yang akan dihapus, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpDelete]
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
        return Ok("Data Deleted");
    }

    /*
     *<summary>request HTTP PUT untuk melakukan perubahan data AcccountRole </summary>
     *<param name="accountRoleDto">Data yang akan dijadikan perubahan, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPut]
    public IActionResult UpdateByGuid(AccountRoleDto accountRoleDto)
    {
        //Mendapatkan data AcccountRole berdasarkan guid
        var accountRoleByGuid = _accountRoleRepository.GetByGuid(accountRoleDto.Guid);
        if (accountRoleByGuid is null)
        {
            return NotFound("ID Not Found");
        }

        //Menyimpan data dari parameter ke dalam objek toUpdate, serta dilakukan mapping secara implisit
        AccountRole toUpdate = accountRoleDto;
        
        //Inisialiasi nilai CreatedDate agar tidak ada perubahan dari data awal
        toUpdate.CreatedDate = accountRoleByGuid.CreatedDate;

        //Melakukan Update dengan parameter toUpdate
        var result = _accountRoleRepository.Update(toUpdate);
        if (!result)
        {
            return BadRequest("Failed to Update Date");

        }
        return Ok("Data Updated");
    }

}
