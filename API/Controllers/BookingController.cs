using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingRepository _bookingRepository;

    public BookingController(IBookingRepository accountRepository)
    {
        _bookingRepository = accountRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _bookingRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _bookingRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(Booking booking)
    {
        booking.CreatedDate = DateTime.Now;

        var result = _bookingRepository.Create(booking);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }

    [HttpDelete("{guid}")]
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
        return Ok(result);
    }

    [HttpPut("{guid}")]
    public IActionResult UpdateByGuid(Guid guid, Booking booking)
    {
        var bookingByGuid = _bookingRepository.GetByGuid(guid);
        if (bookingByGuid is null)
        {
            return NotFound("ID Not Found");
        }
        bookingByGuid.StartDate = booking.StartDate;
        bookingByGuid.EndDate = booking.EndDate;
        bookingByGuid.Status = booking.Status;
        bookingByGuid.Remarks = booking.Remarks;
        bookingByGuid.RoomGuid = booking.RoomGuid;
        bookingByGuid.EmployeeGuid = booking.EmployeeGuid;
 
        bookingByGuid.ModifiedDate = DateTime.Now;

        var result = _bookingRepository.Update(bookingByGuid);
        if (!result)
        {
            return BadRequest("Failed to Update Date");

        }
        return Ok(result);
    }

}
