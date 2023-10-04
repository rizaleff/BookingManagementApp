namespace API.Utilities.Handlers;
/*
 * Definisi kelas ExceptionHandler yang merupakan turunan dari kelas bawaan yaitu Exception
 * Kelas ini digunakan untuk menangkap pesan Error yang ada pada catch
 */
public class ExceptionHandler : Exception
{
    /*
     * Constructor kelas ExceptionHandler
     * Parameter constructor ini merupan pesan error
     * Konstruktor ini memanggil konstruktor pada super class nya
     */
    public ExceptionHandler(string message) : base(message) { }
}

