namespace API.Utilities.Handlers;
public class GenerateHandler
{
    public static string GenerateNik(string lastNik)
    {
        if (lastNik == "") return "111111";

        int nik = Convert.ToInt32(lastNik);
        nik += 1;
        return nik.ToString();
    }

}