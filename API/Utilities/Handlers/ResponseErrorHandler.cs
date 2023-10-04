namespace API.Utilities.Handlers;
/*
 * kelas ResponseErrorHandler digunakan untuk setiap response dengan status Error
 */
public class ResponseErrorHandler
{
    //Deklarasi atribut
    public int Code { get; set; }
    public string Status { get; set; }
    public string Message { get; set; }
    public string? Error { get; set; }
}
