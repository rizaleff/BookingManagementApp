using API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace API.DTOs.Bookings;
public class CreateBookingDto
{
    //Deklarasi atribut yang dibutuhkan sebagai DTO
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Status { get; set; }
    public string Remarks { get; set; }
    public Guid RoomGuid { get; set; }
    public Guid EmployeeGuid { get; set; }

    /*
     *<summary>Implicit opertor untuk mapping dari CreateBookingDto ke Booking secara implisit<summary>
     *<param name="createBookingDto>Object CreateBookingDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object Booking</return>
     */
    public static implicit operator Booking(CreateBookingDto createBookingDto)
    {
        return new Booking
        {
            StartDate = createBookingDto.StartDate,
            EndDate = createBookingDto.EndDate,
            Status = createBookingDto.Status,
            Remarks = createBookingDto.Remarks,
            RoomGuid = createBookingDto.RoomGuid,
            EmployeeGuid = createBookingDto.EmployeeGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

}
