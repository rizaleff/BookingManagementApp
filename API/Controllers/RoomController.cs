using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomRepository _roomRepository;

    public RoomController(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _roomRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _roomRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(Room room)
    {
        room.CreatedDate = DateTime.Now;
        room.ModifiedDate = room.CreatedDate;

        var result = _roomRepository.Create(room);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }

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
        return Ok(result);
    }

    [HttpPut]
    public IActionResult UpdateByGuid(Room room)
    {
        var roomByGuid = _roomRepository.GetByGuid(room.Guid);
        if (roomByGuid is null)
        {
            return NotFound("ID Not Found");
        }
        roomByGuid.Name = room.Name;
        roomByGuid.Floor = room.Floor;
        roomByGuid.Capacity = room.Capacity;

        roomByGuid.ModifiedDate = DateTime.Now;

        var result = _roomRepository.Update(roomByGuid);
        if (!result)
        {
            return BadRequest("Failed to Update Date");

        }
        return Ok(result);
    }

}
