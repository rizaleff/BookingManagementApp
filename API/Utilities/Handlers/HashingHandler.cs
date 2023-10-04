namespace API.Utilities.Handlers;
public class HashingHandler
{
    /*
     * <summary>Digunakan untuk generate Salt secara acak</summary>
     * <returns>Mengembalikan nilai Salt berupa string</returns>
     */
    private static string GetRandomSalt()
    {
        return BCrypt.Net.BCrypt.GenerateSalt(12); //generate salt dengan pengacakan sebanyak 12 kali
    }

    /*
     * <summary>Menghash password dengan salt yang digenerate secara acak</summary>
     * <params name="password">Password merupakan nilai berupa string yang akan di lakukan hashing</params>
     * <returns>Mengembalikan password yang telah di-hash dengan nilai kembalian string<returns>
     */
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());//hasing password
    }

    /*
     * <summary>Melakukan verifikasi password yang diinput dengan passowrd yang telah di-hashing</summary>
     * <params name="password">Password yang dimasukkan untuk dilakukan pencocokan<params>
     * <params name="hashPassword">Password yang telah di-hashing dan dijadikan sebagai acuan pencocokan</summary>
     */
    public static bool VerifyPassword(string password, string hashPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashPassword); //verifikasi password
    }
}
