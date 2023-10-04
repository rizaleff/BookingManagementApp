using API.Contracts;
using API.DTOs.Bookings;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;
[ApiController] //Menandadakan bahwa kelas ini merupakan sebuah controller API
[Route("api/[controller]")] //format route dari tiap endpoint pada controller ini

//Deklarasi kelas BookingController yang merupakan turunan dari kelas ControllerBase
public class BookingController : ControllerBase
{
    //sebagai perantara untuk melakukan CRUD melalui contract yang telah dibuat
    private readonly IBookingRepository _bookingRepository;

    public BookingController(IBookingRepository accountRepository)
    {
        _bookingRepository = accountRepository;
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data dari semua Booking</summary>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpGet]
    public IActionResult GetAll()
    {
        //Mendapatkan data Booking dan disimpan pada variabel result
        var result = _bookingRepository.GetAll();
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

        //mapping setiap item variabel result ke dalam object dari kelas BookingDto menggunakan explicit operator
        var data = result.Select(x => (BookingDto)x);

        //Mengembalikan nilai berupa objek ResponseOKHandler dengan argument <IEnumerable<BookingDto>
        return Ok(new ResponseOKHandler<IEnumerable<BookingDto>>(data));
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data berdasarkan Guid yang dimasukkan pada parameter</summary>
     *<param name="guid">guid yang didapatkan dari path<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        //Mendapatkan data Booking berdasrkan Gui dan disimpan pada variabel result
        var result = _bookingRepository.GetByGuid(guid);

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

        //mapping variabel result ke BookingDto menggunakan explicit operator
        //Mengembalikan nilai berupa response OK dengan response body berupa objek ResponseOKHandler
        return Ok(new ResponseOKHandler<BookingDto>((BookingDto)result));
    }

    /*
     *<summary>request HTTP POST untuk menambahkan Booking baru</summary>
     *<param name="createBookingDto">Data yang akan ditambahkan, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPost]
    public IActionResult Create(CreateBookingDto createBookingDto)
    {
        try
        {
            //Mapping secara implisit pada createBookingDto untuk dijadikan objek Booking
            var result = _bookingRepository.Create(createBookingDto);

            //Mapping variabel result ke BookingDto menggunakan explicit operator
            //Mengembalikan nilai berupa response OK dengan response body berupa objek ResponseOKHandler
            return Ok(new ResponseOKHandler<BookingDto>((BookingDto)result));
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
            //Mendapatkan data Booking berdasrkan Guid dan disimpan pada variabel bookingByGuid
            var bookingByGuid = _bookingRepository.GetByGuid(guid);

            //Mengecek apakah bookingByGuid bernilai null
            if (bookingByGuid is null)
            {
                //Mengembalikan nilai NotFound dengan response body berupa objek ResponseErrorHandler
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound, //Inisialisasi atribut Code dengan nilai 500
                    Status = HttpStatusCode.NotFound.ToString(), //Inisialisasi atribut Status dengan nilai NotFound
                    Message = "Data Not Found" //Inisialisasi nilai atribut Message
                });
            }

            //Melakuan Delete data booking melalui perantara contract _bookingRepository
            _bookingRepository.Delete(bookingByGuid);

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
     *<summary>request HTTP PUT untuk melakukan perubahan data Booking </summary>
     *<param name="bookingDto">Data yang akan dijadikan perubahan, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPut]
    public IActionResult UpdateByGuid(BookingDto bookingDto)
    {
        try
        {
            //Mendapatkan data Booking berdasarkan guid
            var bookingByGuid = _bookingRepository.GetByGuid(bookingDto.Guid);

            //Mengecek apakah bookingByGuid bernilai null
            if (bookingByGuid is null)
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
            Booking toUpdate = bookingDto;
        
            //Inisialiasi nilai CreatedDate agar tidak ada perubahan dari data awal
            toUpdate.CreatedDate = bookingByGuid.CreatedDate;

            //Melakukan Update dengan parameter toUpdate
            var result = _bookingRepository.Update(toUpdate);

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
