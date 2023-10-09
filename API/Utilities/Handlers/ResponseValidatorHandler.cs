using System.Net;

namespace API.Utilities.Handlers;

/*
 * Kelas ResponseValidatorHandler untuk menampung hasil validasi menggunakan Fluent Validation
 */
public class ResponseValidatorHandler
{
    //Deklarasi atribut
    public int Code { get; set; }
    public string Status { get; set; }
    public string Message { get; set; }
    public object Error { get; set; }

    /*
     * Constructor dengan parameter error yang berupa object
     */
    public ResponseValidatorHandler(object error)
    {
        Code = StatusCodes.Status400BadRequest;
        Status = HttpStatusCode.BadRequest.ToString();
        Message = "Validation Error";
        Error = error; //Kumpulan pesan error dari Fluent Validation
    }
}
