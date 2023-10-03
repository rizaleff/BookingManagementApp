using API.Contracts;
using API.DTOs.Accounts;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController] //Menandadakan bahwa kelas ini merupakan sebuah controller API
[Route("api/[controller]")] //format route dari tiap endpoint pada controller ini

//Deklarasi kelas AccountController yang merupakan turunan dari kelas ControllerBase
public class AccountController : ControllerBase 
{
    //sebagai perantara untuk melakukan CRUD melalui contract yang telah dibuat
    private readonly IAccountRepository _accountRepository; 

    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data dari semua Account</summary>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpGet]
    public IActionResult GetAll()
    {
        //Mendapatkan data Account dan disimpan pada variabel result
        var result = _accountRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found"); 
        }

        //mapping setiap item variabel result ke dalam object dari kelas AccountDto menggunakan explicit operator
        var data = result.Select(x => (AccountDto)x); 

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
        var result = _accountRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        //mapping variabel result ke AccountDto menggunakan explicit operator
        return Ok((AccountDto)result); 
    }

    /*
     *<summary>request HTTP POST untuk menambahkan Account baru</summary>
     *<param name="createAccountDto">Data yang akan ditambahkan, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPost]
    public IActionResult Create(CreateAccountDto createAccountDto)
    {
        //Hashing
        Account toCreate = createAccountDto;
        toCreate.Password = HashingHandler.HashPassword(toCreate.Password);

        //Mapping secara implisit pada createAccountDto untuk dijadikan objek Account
        var result = _accountRepository.Create(toCreate);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }
        //Mapping variabel result ke AccountDto menggunakan explicit operator
        return Ok((AccountDto)result);
    }

    /*
     *<summary>request HTTP DELETE untuk menambahkan menghapus data berdasarkan Guid</summary>
     *<param name="guid">Guid dari data yang akan dihapus, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpDelete]
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
        return Ok("Data Deleted");
    }

    /*
     *<summary>request HTTP PUT untuk melakukan perubahan data Account </summary>
     *<param name="accountdto">Data yang akan dijadikan perubahan, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPut]
    public IActionResult UpdateByGuid(AccountDto accountdto)
    {
        //Mendapatkan data Account berdasarkan guid
        var accountByGuid = _accountRepository.GetByGuid(accountdto.Guid);
        if (accountByGuid is null)
        {
            return NotFound("ID Not Found");
        }
        //Menyimpan data dari parameter ke dalam objek toUpdate, serta dilakukan mapping secara implisit
        Account toUpdate = accountdto;

        //Inisialiasi nilai CreatedDate agar tidak ada perubahan dari data awal
        toUpdate.CreatedDate = accountByGuid.CreatedDate; 

        //Melakukan Update dengan parameter toUpdate
        var result = _accountRepository.Update(toUpdate);
        if (!result)
        {
            return BadRequest("Failed to Update Date");

        }
        return Ok("Data Updated");
    }
}
