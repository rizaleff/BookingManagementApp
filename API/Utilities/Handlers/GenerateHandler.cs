namespace API.Utilities.Handlers;
public class GenerateHandler
{

    /*
     * <summary>Generate NIK secara otomatis berdasarkan nilai NIK Terakhir</summary>
     * <param name="lastNik"> NIK terakhir atau empty string jika data pada employee masih kosong</param>
     * <returns>Bernilai string yang merepresentasikan NIK baru</returns>
     */
    public static string GenerateNik(string lastNik)
        {
        //Jika lastNik bernilai empty string maka akan mengembalikan nilai "111111"
        if (lastNik == "") return "111111";

        //Jika lastNik bukan empy string maka akan mengkonversinya ke NIK dan nilainya ditambah 1
        int nik = Convert.ToInt32(lastNik);
        nik += 1;

        //mengembalikan nilai berupa NIK yang dikonversi ke string
        return nik.ToString();
    }


    public static int GenerateOtp()
    {
        Random random = new Random();
        int otp = random.Next(100000, 999999);
        return otp;
    }
    
    //Method untuk generate durasi hari berdasarkan start date dan end date
    public static int GenerateDayLength(DateTime startDate, DateTime endDate)
    {
        //Instansiasi variabel dayLength
        int dayLength = 0;

        //Perulangan untuk menghitung durasi hari
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            //Mengabaikan hari sabtu dan Minggu
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
            {
                dayLength++;
            }
        }
        return dayLength;
    }
}
