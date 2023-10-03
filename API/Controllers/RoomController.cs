using API.Contracts;
using API.DTOs.Rooms;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController] //Menandadakan bahwa kelas ini merupakan sebuah controller API
[Route("api/[controller]")] //format route dari tiap endpoint pada controller ini

//Deklarasi kelas RoomController yang merupakan turunan dari kelas ControllerBase
public class RoomController : ControllerBase
{
    //sebagai perantara untuk melakukan CRUD melalui contract yang telah dibuat
    private readonly IRoomRepository _roomRepository;

    public RoomController(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    /*
     *<summary>request HTTP GET untuk mendpatkan data dari semua Room</summary>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpGet]
    public IActionResult GetAll()
    {
        //Mendapatkan data Room dan disimpan pada variabel result
        var result = _roomRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        //mapping setiap item variabel result ke dalam object dari kelas RoomDto menggunakan explicit operator
        var data = result.Select(x => (RoomDto)x);
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
        var result = _roomRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        //mapping variabel result ke RoomDto menggunakan explicit operator
        return Ok((RoomDto)result);
    }

    /*
     *<summary>request HTTP POST untuk menambahkan Room baru</summary>
     *<param name="createRoomDto">Data yang akan ditambahkan, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPost]
    public IActionResult Create(CreateRoomDto createRoomDto)
    {

        //Mapping secara implisit pada createRoomDto untuk dijadikan objek Room
        var result = _roomRepository.Create(createRoomDto);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        //Mapping variabel result ke RoomDto menggunakan explicit operator
        return Ok((RoomDto)result);
    }

    /*
     *<summary>request HTTP DELETE untuk menambahkan menghapus data berdasarkan Guid</summary>
     *<param name="guid">Guid dari data yang akan dihapus, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var roomByGuid = _roomRepository.GetByGuid(guid);
        if (roomByGuid is null)
        {
            return NotFound("ID Not Found");
        }
        var result = _roomRepository.Delete(roomByGuid);
        if (!result)
        {
            return BadRequest("Failed to delete data");
        }
        return Ok("Data Deleted");
    }

    /*
     *<summary>request HTTP PUT untuk melakukan perubahan data Room </summary>
     *<param name="roomDto">Data yang akan dijadikan perubahan, didapatkan dari request body</param>
     *<returns>return value berupa status HTTP response status codes</returns> 
     */
    [HttpPut]
    public IActionResult UpdateByGuid(RoomDto roomDto)
    {
        var roomByGuid = _roomRepository.GetByGuid(roomDto.Guid);
        if (roomByGuid is null)
        {
            return NotFound("ID Not Found");
        }

        //Mendapatkan data Room berdasarkan guid
        Room toUpdate = roomDto;
        
        //Inisialiasi nilai CreatedDate agar tidak ada perubahan dari data awal
        toUpdate.CreatedDate = roomByGuid.CreatedDate;

        //Melakukan Update dengan parameter toUpdate
        var result = _roomRepository.Update(toUpdate);
        if (!result)
        {
            return BadRequest("Failed to Update Date");

        }
        return Ok("Data Updated");
    }

}
