namespace API.DTOs.Bookings;
/*
 * Class : BookingTodayDto
 * Mendefinisikan properties yang disajikan
 * Untuk mengetahui room apa saja yang digunakan hari ini
 */
public class BookingDetailDto
{
    public Guid Guid {  get; set; }
    public string BookedNik { get; set; }
    public string BookedBy { get; set; }
    public string RoomName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }
    public string Remarks { get; set; }
}
