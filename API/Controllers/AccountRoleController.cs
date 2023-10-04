using API.Contracts;
using API.DTOs.AccountRoles;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
            //Mengembalikan nilai dengan response body berupa objek ResponseErrorHandler
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound, //Inisialisasi nilai atribut Code
                Status = HttpStatusCode.NotFound.ToString(), //Inisialisai nilai atribut Status
                Message = "Data Not Found" //Inisialisasi nilai atribut Message
            });
        }

        //mapping setiap item variabel result ke dalam object dari kelas AccountRoleDto menggunakan explicit operator
        var data = result.Select(x => (AccountRoleDto)x);

        //Mengembalikan nilai berupa objek ResponseOKHandler dengan argument <IEnumerable<AccountRoleDto>
        return Ok(new ResponseOKHandler<IEnumerable<AccountRoleDto>>(data));
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data berdasarkan Guid yang dimasukkan pada parameter</summary>
     *<param name="guid">guid yang didapatkan dari path<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        //Mendapatkan data AccountRole berdasrkan Gui dan disimpan pada variabel result
        var result = _accountRoleRepository.GetByGuid(guid);
        //Mengecek apakah variabel result bernilai null
        if (result is null)
        {
            //Mengembalikan nilai dengan response body berupa objek ResponseErrorHandler
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound, //Inisialisasi nilai atribut Code
                Status = HttpStatusCode.NotFound.ToString(), //Inisialisai nilai atribut Status
                Message = "Data Not Found" //Inisialisasi nilai atribut Message
            });
        }
        //mapping variabel result ke AccountRoleDto menggunakan explicit operator
        //Mengembalikan nilai berupa response OK dengan response body berupa objek ResponseOKHandler
        return Ok(new ResponseOKHandler<AccountRoleDto>((AccountRoleDto)result));
    }

    /*
     *<summary>request HTTP POST untuk menambahkan AcccountRole baru</summary>
     *<param name="createAccountRoleDto">Data yang akan ditambahkan, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPost]
    public IActionResult Create(CreateAccountRoleDto createAccountRoleDto)
    {
        try
        {
            //Mapping secara implisit pada createAccountleDto untuk dijadikan objek AcccountRole
            var result = _accountRoleRepository.Create(createAccountRoleDto);

            //Mapping variabel result ke AccountRoleDto menggunakan explicit operator
            //Mengembalikan nilai berupa response OK dengan response body berupa objek ResponseOKHandler
            return Ok(new ResponseOKHandler<AccountRoleDto>((AccountRoleDto)result));
        }
        catch (ExceptionHandler ex)
        {
            //Mengembalikan nilai dengan response body berupa objek ResponseErrorHandler
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,//Inisialisasi atribut Code dengan nilai 500
                Status = HttpStatusCode.InternalServerError.ToString(), //Inisialisasi atribut Status dengan nilai InternalServerError
                Message = "Failed to Create Data", //Inisialisasi nilai atribut Message
                Error = ex.Message  //Inisialisasi nilai atribut Error berupa Message dari ExceptionHandler
            });
        }
    }

    /*
     *<summary>request HTTP DELETE untuk menambahkan menghapus data berdasarkan Guid</summary>
     *<param name="guid">Guid dari data yang akan dihapus, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        try
        {
            //Mendapatkan data Account berdasrkan Gui dan disimpan pada variabel result
            var accountRoleByGuid = _accountRoleRepository.GetByGuid(guid);

            //Mengecek apakah accountRoleByGuid bernilai null
            if (accountRoleByGuid is null)
            {
                //Mengembalikan nilai NotFound dengan response body berupa objek ResponseErrorHandler
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound, //Inisialisasi atribut Code dengan nilai 500
                    Status = HttpStatusCode.NotFound.ToString(), //Inisialisasi atribut Status dengan nilai NotFound
                    Message = "Data Not Found" //Inisialisasi nilai atribut Message
                });
            }

            //Melakuan Delete data AccountRole melalui perantara contract _accountRoleRepository
            _accountRoleRepository.Delete(accountRoleByGuid);
            
            //Mengembalikan nilai berupa response OK dengan response body berupa objek ResponseOKHandler
            return Ok(new ResponseOKHandler<string>("Data Deleted"));

        }
        catch (ExceptionHandler ex)
        {
            //Mengembalikan nilai berupa Respon Status 500 dan objek ResponseErrorHandler
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError, //Inisialiasi atribut Code dengan nilai 500
                Status = HttpStatusCode.InternalServerError.ToString(), //Inisialisasi atribut Status dengan nilai InternalServerError
                Message = "Failed to create data", //Inisialisasi atribut Message
                Error = ex.Message //Inisialisasi nilai atribut Error berupa Message dari ExceptionHandler
            });
        }
    }

    /*
     *<summary>request HTTP PUT untuk melakukan perubahan data AcccountRole </summary>
     *<param name="accountRoleDto">Data yang akan dijadikan perubahan, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPut]
    public IActionResult UpdateByGuid(AccountRoleDto accountRoleDto)
    {
        try
        {
            //Mendapatkan data AcccountRole berdasarkan guid
            var accountRoleByGuid = _accountRoleRepository.GetByGuid(accountRoleDto.Guid);

            //Mengecek apakah accountRoleByGuid bernilai null
            if (accountRoleByGuid is null)
            {
                //Mengembalikan nilai response NotFound dengan response body berupa objek ResponseErrorHandler
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound, //Inisialisasi atribut Code dengan nilai 500
                    Status = HttpStatusCode.NotFound.ToString(), //Inisialisasi atribut Status dengan nilai NotFound
                    Message = "Data Not Found" //Inisialisasi nilai atribut Message
                });
            }

            //Menyimpan data dari parameter ke dalam objek toUpdate, serta dilakukan mapping secara implisit
            AccountRole toUpdate = accountRoleDto;
        
            //Inisialiasi nilai CreatedDate agar tidak ada perubahan dari data awal
            toUpdate.CreatedDate = accountRoleByGuid.CreatedDate;

            //Melakukan Update dengan parameter toUpdate
            _accountRoleRepository.Update(toUpdate);

           //Mengembalikan nilai response OK dengan response body berupa objek ResponseOKHandler dengan argumen string
            return Ok(new ResponseOKHandler<string>("Data Updated"));
        }
        catch (ExceptionHandler ex)
        {
            //Mengembalikan nilai berupa Respon Status 500 dan objek ResponseErrorHandler
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError, //Inisialiasi atribut Code dengan nilai 500
                Status = HttpStatusCode.InternalServerError.ToString(), //Inisialisasi atribut Status dengan nilai InternalServerError
                Message = "Failed to create data", //Inisialisasi atribut Message
                Error = ex.Message //Inisialisasi nilai atribut Error berupa Message dari ExceptionHandler
            });
        }
    }

}
