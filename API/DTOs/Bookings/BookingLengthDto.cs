namespace API.DTOs.Bookings;
/*
 * Class : BookingTodayDto
 * Mendefinisikan properties yang dibutuhkan
 */
public class BookingLengthDto
{
    public Guid RoomGuid {  get; set; }
    public string RoomName { get; set;}
    public int BookingLength {  get; set;}
}
