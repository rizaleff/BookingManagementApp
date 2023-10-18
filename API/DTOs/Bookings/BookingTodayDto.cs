namespace API.DTOs.Bookings;
/*
 * Class : BookingTodayDto
 * Mendefinisikan properties yang disajikan
 * Untuk mengetahui room apa saja yang digunakan hari ini
 */
public class BookingTodayDto
{
    public Guid BookingGuid { get; set; }
    public string RoomName {  get; set; }
    public string Status {  get; set; }
    public int Floor {  get; set; }
    public string BookedBy {  get; set; }   

}

