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

}