using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Bookings;
public class BookingDto
{
    //Deklarasi atribut yang dibutuhkan sebagai DTO
    public Guid Guid { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public StatusLevel Status { get; set; }
    public string Remarks { get; set; }
    public Guid RoomGuid { get; set; }
    public Guid EmployeeGuid { get; set; }

    /*
     *<summary>Implicit opertor untuk mapping dari BookingDto ke Booking secara implisit<summary>
     *<param name="bookingDto>Object BookingDto yang akan di Mapping</param>
     *<return>Hasil mapping berupa object Booking</return>
     */
    public static implicit operator Booking(BookingDto bookingDto)
    {
        return new Booking
        {
            Guid = bookingDto.Guid,
            StartDate = bookingDto.StartDate,
            EndDate = bookingDto.EndDate,
            Status = bookingDto.Status,
            Remarks = bookingDto.Remarks,
            RoomGuid = bookingDto.RoomGuid,
            EmployeeGuid = bookingDto.EmployeeGuid,
            ModifiedDate = DateTime.Now
        };
    }
    /*
     *<summary>Explicit opertor untuk mapping dari Booking ke BookingDto secara eksplisit<summary>
     *<param name="booking>Object Booking yang akan di Mapping</param>
     *<return>Hasil mapping berupa object BookingDto</return>
     */
    public static explicit operator BookingDto(Booking booking)
    {
        return new BookingDto
        {
            Guid = booking.Guid,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Status = booking.Status,
            Remarks = booking.Remarks,
            RoomGuid = booking.RoomGuid,
            EmployeeGuid = booking.EmployeeGuid
        };
    }
}
