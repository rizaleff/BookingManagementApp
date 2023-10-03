using API.Models;

namespace API.DTOs.Rooms;
public class RoomDto
{
    //Deklarasi atribut yang dibutuhkan sebagai DTO
    public Guid Guid {get; set;}
    public string Name { get; set; }
    public int Floor { get; set; }
    public int Capacity { get; set; }

    /*
     *<summary>Implicit opertor untuk mapping dari RoomDto ke Room secara implisit<summary>
     *<param name="roomDto>Object RoomDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object Room</return>
     */
    public static implicit operator Room(RoomDto roomDto)
    {
        return new Room
        {
            Guid = roomDto.Guid,
            Name = roomDto.Name,
            Floor = roomDto.Floor,
            Capacity = roomDto.Capacity,
            ModifiedDate = DateTime.Now
        };
    }

    /*
     *<summary>explicit operator untuk mapping dari Room ke RoomDto secara eksplisit<summary>
     *<param name="room>Object Room yang akan di Mapping</param>
     *<return>Hasil mapping berupa object RoomDto</return>
     */
    public static explicit operator RoomDto(Room room)
    {
        return new RoomDto
        {
            Guid = room.Guid,
            Name = room.Name,
            Floor = room.Floor,
            Capacity = room.Capacity,
        }; //Instansiasi objek sebagai return value
    }
}