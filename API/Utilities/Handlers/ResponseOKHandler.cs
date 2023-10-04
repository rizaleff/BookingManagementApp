using System.Net;

namespace API.Utilities.Handlers;
/*
 * kelas ResponseOKHandler digunakan untuk menangani setiap response dengan status OK
 * Kelas ini memiliki dua constructor untuk mengurangi rendudancy data saat pembuatan objek
 */

public class ResponseOKHandler<TEntity>
{
    //Deklarasi atribug
    public int Code { get; set; }
    public string Status { get; set; }
    public string Message { get; set; }
    public TEntity? Data { get; set; }

    /*
     * <summary>Constructor dengan parameter berupa TEntity atau objek</summary>
     * <param name="data">parameter beritpe generic disesuaikan dengan kebutuhannya</param>
     */
    public ResponseOKHandler(TEntity? data)
    {
        Code = StatusCodes.Status200OK; //Inisialiasi nilai atribut Code dengan nlai status code 200
        Status = HttpStatusCode.OK.ToString(); //Inisialiasi nilai atribut Status dengan nlai Ok
        Message = "Success to Retrieve Data"; //Inisialiasi nilai atribut Message
        Data = data; //Inisialisasi nilai atribut Data berdasarkan parameter
    }

    /*
     * <summary>Constructor dengan parameter berupa string</summary>
     * <param name="message">parameter message beritpe string berupa pesan yang akan ditampilkan</param>
     */
    public ResponseOKHandler(string message)
    {
        Code = StatusCodes.Status200OK; //Inisialiasi nilai atribut Code dengan nlai status code 200
        Status = HttpStatusCode.OK.ToString(); //Inisialiasi nilai atribut Status dengan nlai Ok
        Message = "Success to Retrieve Data"; //Inisialiasi nilai atribut Message
    }
}
