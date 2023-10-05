using API.Contracts;
using API.DTOs;
using API.DTOs.Accounts;
using API.DTOs.Educations;
using API.DTOs.Employees;
using API.DTOs.Universities;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Transactions;

namespace API.Controllers;
[ApiController] //Menandadakan bahwa kelas ini merupakan sebuah controller API
[Route("api/[controller]")] //format route dari tiap endpoint pada controller ini

//Deklarasi kelas BookingController yang merupakan turunan dari kelas ControllerBase
public class EmployeeController : ControllerBase
{
    //sebagai perantara untuk melakukan CRUD melalui contract yang telah dibuat
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IAccountRepository _accountRepository;

    public EmployeeController(IEmployeeRepository employeeRepository, IEducationRepository educationRepository, IUniversityRepository universityRepository, IAccountRepository accountRepository)
    {
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _universityRepository = universityRepository;
        _accountRepository = accountRepository;
    }

    [HttpGet("details")]
    public IActionResult GetDetails()
    {
        var employees = _employeeRepository.GetAll();
        var educations = _educationRepository.GetAll();
        var universities = _universityRepository.GetAll();

        if (!(employees.Any() && educations.Any() && universities.Any()))
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        var employeeDetails = from emp in employees
                              join edu in educations on emp.Guid equals edu.Guid
                              join unv in universities on edu.UniversityGuid equals unv.Guid
                              select new EmployeeDetailDto
                              {
                                  Guid = emp.Guid,
                                  Nik = emp.Nik,
                                  FullName = string.Concat(emp.FirstName, " ", emp.LastName),
                                  BirthDate = emp.BirthDate,
                                  Gender = emp.Gender.ToString(),
                                  HiringDate = emp.HiringDate,
                                  Email = emp.Email,
                                  PhoneNumber = emp.PhoneNumber,
                                  Major = edu.Major,
                                  Degree = edu.Degree,
                                  Gpa = edu.Gpa,
                                  University = unv.Name
                              };

        return Ok(new ResponseOKHandler<IEnumerable<EmployeeDetailDto>>(employeeDetails));
    }


    [HttpPost("Register")]
    public IActionResult Register(RegisterEmployeeDto registerEmployeeDto)
    {

        try
        {
            using (var transaction = new TransactionScope())
            {
                Guid univGuid = _universityRepository.UniversityGuidByName(registerEmployeeDto.UniversityName);
                if (univGuid == Guid.Empty)
                {
                    //Mapping secara implisit pada createUniversityDto untuk dijadikan objek University
                    University toCreateUniv = new CreateUniversityDto
                    {
                        Code = registerEmployeeDto.UniversityCode,
                        Name = registerEmployeeDto.UniversityName
                    };
                    var resultUniversity = _universityRepository.Create(toCreateUniv);
                    univGuid = resultUniversity.Guid;
                }

                Employee toCreateEmployee = new CreateEmployeeDto
                {
                    FirstName = registerEmployeeDto.FirstName,
                    LastName = registerEmployeeDto.LastName,
                    BirthDate = registerEmployeeDto.BirthDate,
                    Email = registerEmployeeDto.Email,
                    Gender = registerEmployeeDto.Gender,
                    HiringDate = registerEmployeeDto.HiringDate,
                    PhoneNumber = registerEmployeeDto.PhoneNumber
                };

                toCreateEmployee.Nik = GenerateHandler.GenerateNik(_employeeRepository.GetLastNik());

                var resultEmployee = _employeeRepository.Create(toCreateEmployee);

                Education toCreateEducation = new CreateEducationDto
                {
                    Degree = registerEmployeeDto.Degree,
                    Gpa = registerEmployeeDto.Gpa,
                    Major = registerEmployeeDto.Major,
                    Guid = resultEmployee.Guid,
                    UniversityGuid = univGuid
                };

                _educationRepository.Create(toCreateEducation);


                Account toCreateAccount = new CreateAccountDto
                {
                    Guid = resultEmployee.Guid,
                    Password = registerEmployeeDto.Password

                };



                toCreateAccount.Password = HashingHandler.HashPassword(toCreateAccount.Password);

                var resultAccount = _accountRepository.Create(toCreateAccount);

                transaction.Complete();
                return Ok("Sukses");
            };

            
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
            //Mengembalikan nilai dengan response body berupa objek ResponseErrorHandler
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound, //Inisialisasi nilai atribut Code
                Status = HttpStatusCode.NotFound.ToString(), //Inisialisai nilai atribut Status
                Message = "Data Not Found" //Inisialisasi nilai atribut Message
            });
        }

        //mapping setiap item variabel result ke dalam object dari kelas EmployeeDto menggunakan explicit operator
        var data = result.Select(x => (EmployeeDto)x);

        //Mengembalikan nilai berupa objek ResponseOKHandler dengan argument <IEnumerable<EmployeeDto>
        return Ok(new ResponseOKHandler<IEnumerable<EmployeeDto>>(data));
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data berdasarkan Guid yang dimasukkan pada parameter</summary>
     *<param name="guid">guid yang didapatkan dari path</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        //Mendapatkan data Employee berdasrkan Guid dan disimpan pada variabel result
        var result = _employeeRepository.GetByGuid(guid);

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

        //mapping variabel result ke EmployeeDto menggunakan explicit operator
        //Mengembalikan nilai berupa response OK dengan response body berupa objek ResponseOKHandler
        return Ok(new ResponseOKHandler<EmployeeDto>((EmployeeDto)result));
    }

    /*
     *<summary>request HTTP POST untuk menambahkan Employee baru</summary>
     *<param name="createEmployeeDto">Data yang akan ditambahkan, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPost]
    public IActionResult Create(CreateEmployeeDto employeeDto)
    {
        try
        {
            //Instantiasi objek toCreate dengan nilai sesuai parameter employeeDto
            Employee toCreate = employeeDto;

            //Inisialisasi nilai atribut NIK pada objek toCreate melalui method GenerateNIK
            toCreate.Nik = GenerateHandler.GenerateNik(_employeeRepository.GetLastNik());

            //Mapping secara implisit pada createEmployeeDto untuk dijadikan objek Employee
            var result = _employeeRepository.Create(toCreate);

            //Mapping variabel result ke EmployeeDto menggunakan explicit operator
            //Mengembalikan nilai berupa response OK dengan response body berupa objek ResponseOKHandler
            return Ok(new ResponseOKHandler<EmployeeDto>((EmployeeDto)result));
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
            //Mendapatkan data Employee berdasrkan Guid dan disimpan pada variabel employeeByGuid
            var employeeByGuid = _employeeRepository.GetByGuid(guid);
            
            //Mengecek apakah employeeByGuid bernilai null
            if (employeeByGuid is null)
            {
                //Mengembalikan nilai NotFound dengan response body berupa objek ResponseErrorHandler
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound, //Inisialisasi atribut Code dengan nilai 500
                    Status = HttpStatusCode.NotFound.ToString(), //Inisialisasi atribut Status dengan nilai NotFound
                    Message = "Data Not Found" //Inisialisasi nilai atribut Message
                });
            }
        
            //Melakuan Delete data employee melalui perantara contract _employeeRepository
            _employeeRepository.Delete(employeeByGuid);

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
     *<summary>request HTTP PUT untuk melakukan perubahan data Employee </summary>
     *<param name="employeeDto">Data yang akan dijadikan perubahan, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPut]
    public IActionResult UpdateByGuid(EmployeeDto employeeDto)
    {
        try
        {
            //Mendapatkan data Employee berdasarkan guid dan disimpan pada variabel employeeByGuid
            var employeeByGuid = _employeeRepository.GetByGuid(employeeDto.Guid);

            //Mengecek apakah employeeByGuid bernilai null
            if (employeeByGuid is null)
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
            Employee toUpdate = employeeDto;
            toUpdate.Nik = employeeByGuid.Nik;

            //Inisialiasi nilai CreatedDate agar tidak ada perubahan dari data awal
            toUpdate.CreatedDate = employeeByGuid.CreatedDate;

            //Melaukan update pada data employee melalui perantara contract _employeeRepository
            _employeeRepository.Update(toUpdate);

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
