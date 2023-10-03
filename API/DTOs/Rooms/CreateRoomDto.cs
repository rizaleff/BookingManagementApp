using API.Models;

namespace API.DTOs.Rooms;
public class CreateRoomDto
{
    //Deklarasi atribut yang dibutuhkan sebagai DTO
    public string Name { get; set; }
    public int Floor {  get; set; }
    public int Capacity { get; set; }


    /*
     * <summary>Implicit opertor digunakan untuk mapping dari CreateRoomDto ke Room secara implisit<summary>
     *<param name="createRoomDto>Object dari CreatRoomDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object Room</return>
     */
    public static implicit operator Room(CreateRoomDto createRoomDto)
    {
        return new Room
        {
            Name = createRoomDto.Name,
            Floor = createRoomDto.Floor,
            Capacity = createRoomDto.Capacity, 
            CreatedDate = DateTime.Now, 
            ModifiedDate = DateTime.Now
        }; //instansiasi objek sebagai return value
    }
}
