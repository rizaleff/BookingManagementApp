﻿using API.Contracts;
using API.DTOs.Accounts;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController] //Menandadakan bahwa kelas ini merupakan sebuah controller API
[Route("api/[controller]")] //format route dari tiap endpoint pada controller ini

//Deklarasi kelas AccountController yang merupakan turunan dari kelas ControllerBase
public class AccountController : ControllerBase
{
    //sebagai perantara untuk melakukan CRUD melalui contract yang telah dibuat
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmailHandler _emailHandler;

    public AccountController(IAccountRepository accountRepository, IEmployeeRepository employeeRepository, IEmailHandler emailHandler)
    {
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _emailHandler = emailHandler;
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data dari semua Account</summary>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */

    [HttpPost("forgotpassword")]
    public IActionResult GetOtp(string email)
    {
        Guid guid = _employeeRepository.GetGuidByEmail(email);
        if (guid == Guid.Empty)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound, //Inisialisasi nilai atribut Code
                Status = HttpStatusCode.NotFound.ToString(), //Inisialisai nilai atribut Status
                Message = "Email is not registered" //Inisialisasi nilai atribut Message
            });
        }

        try
        {
            //Mendapatkan data Account berdasarkan guid
            var accountByGuid = _accountRepository.GetByGuid(guid);


            //Mengecek apakah accountByGuid bernilai null
            if (accountByGuid is null)
            {
                //Mengembalikan nilai response NotFound dengan response body berupa objek ResponseErrorHandler
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound, //Inisialisasi atribut Code dengan nilai 500
                    Status = HttpStatusCode.NotFound.ToString(), //Inisialisasi atribut Status dengan nilai NotFound
                    Message = "Account Was Not Created. Please register your account first!" //Inisialisasi nilai atribut Message
                });
            }
            //Menyimpan data dari parameter ke dalam objek toUpdate, serta dilakukan mapping secara implisit
            Account toUpdate = new AccountDto
            {
                Guid = guid,
                Otp = GenerateHandler.GenerateOtp(),
                IsUsed = false,
                ExpiredTime = DateTime.Now.AddMinutes(5),
                Password = accountByGuid.Password
            };

            //Inisialiasi nilai CreatedDate agar tidak ada perubahan dari data awal
            toUpdate.CreatedDate = accountByGuid.CreatedDate;

            //Melakukan Update dengan parameter toUpdate
            _accountRepository.Update(toUpdate);

            _emailHandler.Send("Forgot Password", $"Your OTP is {toUpdate.Otp}", email);
            //Mengembalikan nilai response OK dengan response body berupa objek ResponseOKHandler dengan argumen string
            return Ok(new ResponseOKHandler<string>("OTP has been sent to your email");
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


    [HttpPut("changepassword")]
    public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
    {
        Guid guid = _employeeRepository.GetGuidByEmail(changePasswordDto.Email);
        if (guid == Guid.Empty)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound, //Inisialisasi nilai atribut Code
                Status = HttpStatusCode.NotFound.ToString(), //Inisialisai nilai atribut Status
                Message = "Email was not registered" //Inisialisasi nilai atribut Message
            });
        }

        var accountByGuid = _accountRepository.GetByGuid(guid);
        //Mengecek apakah variabel result bernilai null
        if (accountByGuid is null)
        {
            //Mengembalikan nilai dengan response body berupa objek ResponseErrorHandler
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound, //Inisialisasi nilai atribut Code
                Status = HttpStatusCode.NotFound.ToString(), //Inisialisai nilai atribut Status
                Message = "Data Not Found" //Inisialisasi nilai atribut Message
            });
        }

        if (changePasswordDto.Otp != accountByGuid.Otp)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status400BadRequest, //Inisialisasi nilai atribut Code
                Status = HttpStatusCode.BadRequest.ToString(), //Inisialisai nilai atribut Status
                Message = "OTP is not valid" //Inisialisasi nilai atribut Message
            });
        }
        if (accountByGuid.ExpiredTime < DateTime.Now)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status400BadRequest, //Inisialisasi nilai atribut Code
                Status = HttpStatusCode.BadRequest.ToString(), //Inisialisai nilai atribut Status
                Message = "OTP Was Expired" //Inisialisasi nilai atribut Message
            });
        }
        if (accountByGuid.IsUsed)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status400BadRequest, //Inisialisasi nilai atribut Code
                Status = HttpStatusCode.BadRequest.ToString(), //Inisialisai nilai atribut Status
                Message = "OTP was used" //Inisialisasi nilai atribut Message
            });
        }

        Account toUpdate = new AccountDto
        {
            Guid = guid,
            Otp = GenerateHandler.GenerateOtp(),
            IsUsed = true,
            ExpiredTime = accountByGuid.ExpiredTime,
            Password = HashingHandler.HashPassword(changePasswordDto.NewPassword)
        };

        //Inisialiasi nilai CreatedDate agar tidak ada perubahan dari data awal
        toUpdate.CreatedDate = accountByGuid.CreatedDate;

        //Melakukan Update dengan parameter toUpdate
        _accountRepository.Update(toUpdate);

        //Mengembalikan nilai response OK dengan response body berupa objek ResponseOKHandler dengan argumen string
        return Ok(new ResponseOKHandler<string>("Change Password Success"));
    }


    [HttpPost("login")]
    public IActionResult Login(LoginDto loginDto)
    {
        Guid guid = _employeeRepository.GetGuidByEmail(loginDto.Email);
        if (guid == Guid.Empty)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound, //Inisialisasi nilai atribut Code
                Status = HttpStatusCode.NotFound.ToString(), //Inisialisai nilai atribut Status
                Message = "Email was not registered" //Inisialisasi nilai atribut Message
            });
        }

        string? hashPassword = _accountRepository.GetPasswordByGuid(guid);
        if (hashPassword == null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound, //Inisialisasi atribut Code dengan nilai 500
                Status = HttpStatusCode.NotFound.ToString(), //Inisialisasi atribut Status dengan nilai NotFound
                Message = "Account Was Not Created. Please register your account first!" //Inisialisasi nilai atribut Message
            });
        }

        if (!HashingHandler.VerifyPassword(loginDto.Password, hashPassword))
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status400BadRequest, //Inisialisasi nilai atribut Code
                Status = HttpStatusCode.BadRequest.ToString(), //Inisialisai nilai atribut Status
                Message = "Wrong Password" //Inisialisasi nilai atribut Message
            });
        }

        //Mengembalikan nilai response OK dengan response body berupa objek ResponseOKHandler dengan argumen string
        return Ok(new ResponseOKHandler<string>("Login Sukses"));
    }


    [HttpGet]
    public IActionResult GetAll()
    {
        //Mendapatkan data Account dan disimpan pada variabel result
        var result = _accountRepository.GetAll();
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

        //mapping setiap item variabel result ke dalam object dari kelas AccountDto menggunakan explicit operator
        var data = result.Select(x => (AccountDto)x);

        //Mengembalikan nilai berupa objek ResponseOKHandler dengan argument <IEnumerable<AccountDto>
        return Ok(new ResponseOKHandler<IEnumerable<AccountDto>>(data));
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data berdasarkan Guid yang dimasukkan pada parameter</summary>
     *<param name="guid">guid yang didapatkan dari path</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        //Mendapatkan data Account berdasrkan Gui dan disimpan pada variabel result
        var result = _accountRepository.GetByGuid(guid);
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
        ///Mengembalikan nilai response OK dengan response body berupa objek ResponseOKHandler dengan argumen string 
        //mapping variabel result ke AccountDto menggunakan explicit operator
        return Ok(new ResponseOKHandler<AccountDto>((AccountDto)result));
    }

    /*
     *<summary>request HTTP POST untuk menambahkan Account baru</summary>
     *<param name="createAccountDto">Data yang akan ditambahkan, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPost]
    public IActionResult Create(CreateAccountDto createAccountDto)
    {
        try
        {
            //Hashing
            Account toCreate = createAccountDto;
            toCreate.Password = HashingHandler.HashPassword(toCreate.Password);

            //Mapping secara implisit pada createAccountDto untuk dijadikan objek Account
            var result = _accountRepository.Create(toCreate);

            //Mapping variabel result ke AccountDto menggunakan explicit operator
            //Mengembalikan nilai berupa response OK dengan response body berupa objek ResponseOKHandler
            return Ok(new ResponseOKHandler<AccountDto>((AccountDto)result));
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
     *<param name="guid">Guid dari data yang akan dihapus, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        try
        {
            //Mendapatkan data Account berdasrkan Gui dan disimpan pada variabel 
            var accountByGuid = _accountRepository.GetByGuid(guid);

            //Mengecek apakah accountByGuid bernilai null
            if (accountByGuid is null)
            {
                //Mengembalikan nilai NotFound dengan response body berupa objek ResponseErrorHandler
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound, //Inisialisasi atribut Code dengan nilai 500
                    Status = HttpStatusCode.NotFound.ToString(), //Inisialisasi atribut Status dengan nilai NotFound
                    Message = "Data Not Found" //Inisialisasi nilai atribut Message
                });
            }

            //Melakuan Delete data Account melalui perantara contract _accountRepository
            _accountRepository.Delete(accountByGuid);

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
     *<summary>request HTTP PUT untuk melakukan perubahan data Account </summary>
     *<param name="accountdto">Data yang akan dijadikan perubahan, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPut]
    public IActionResult UpdateByGuid(AccountDto accountdto)
    {
        try
        {
            //Mendapatkan data Account berdasarkan guid
            var accountByGuid = _accountRepository.GetByGuid(accountdto.Guid);

            //Mengecek apakah accountByGuid bernilai null
            if (accountByGuid is null)
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
            Account toUpdate = accountdto;

            //Inisialiasi nilai CreatedDate agar tidak ada perubahan dari data awal
            toUpdate.CreatedDate = accountByGuid.CreatedDate;

            //Melakukan Update dengan parameter toUpdate
            var result = _accountRepository.Update(toUpdate);

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
