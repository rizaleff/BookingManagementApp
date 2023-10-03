using API.Contracts;
using API.DTOs.Bookings;
using API.Models;
using Microsoft.AspNetCore.Mvc;

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
            return NotFound("Data Not Found");
        }

        //mapping setiap item variabel result ke dalam object dari kelas BookingDto menggunakan explicit operator
        var data = result.Select(x => (BookingDto)x);

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
        var result = _bookingRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }

        //mapping variabel result ke BookingDto menggunakan explicit operator
        return Ok((BookingDto)result);
    }

    /*
     *<summary>request HTTP POST untuk menambahkan Booking baru</summary>
     *<param name="createBookingDto">Data yang akan ditambahkan, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPost]
    public IActionResult Create(CreateBookingDto createBookingDto)
    {
        //Mapping secara implisit pada createBookingDto untuk dijadikan objek Booking
        var result = _bookingRepository.Create(createBookingDto);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        //Mapping variabel result ke BookingDto menggunakan explicit operator
        return Ok((BookingDto)result);
    }

    /*
     *<summary>request HTTP DELETE untuk menambahkan menghapus data berdasarkan Guid</summary>
     *<param name="guid">Guid dari data yang akan dihapus, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var bookingByGuid = _bookingRepository.GetByGuid(guid);
        if (bookingByGuid is null)
        {
            return NotFound("ID Not Found");
        }
        var result = _bookingRepository.Delete(bookingByGuid);
        if (!result)
        {
            return BadRequest("Failed to delete data");
        }
        return Ok("Data Deleted");
    }

    /*
     *<summary>request HTTP PUT untuk melakukan perubahan data Booking </summary>
     *<param name="bookingDto">Data yang akan dijadikan perubahan, didapatkan dari request body<param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPut]
    public IActionResult UpdateByGuid(BookingDto bookingDto)
    {
        //Mendapatkan data Booking berdasarkan guid
        var bookingByGuid = _bookingRepository.GetByGuid(bookingDto.Guid);
        if (bookingByGuid is null)
        {
            return NotFound("ID Not Found");
        }

        //Menyimpan data dari parameter ke dalam objek toUpdate, serta dilakukan mapping secara implisit
        Booking toUpdate = bookingDto;
        
        //Inisialiasi nilai CreatedDate agar tidak ada perubahan dari data awal
        toUpdate.CreatedDate = bookingByGuid.CreatedDate;

        //Melakukan Update dengan parameter toUpdate
        var result = _bookingRepository.Update(toUpdate);
        if (!result)
        {
            return BadRequest("Failed to Update Date");

        }
        return Ok("Data Updated");
    }

}
