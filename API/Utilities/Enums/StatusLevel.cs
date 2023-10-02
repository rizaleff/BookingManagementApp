using System.ComponentModel.DataAnnotations;

namespace API.Utilities.Enums
{
    /*
     * -Enum di ASP .NET di konversi ke int pada SQL Server. 
     * Ketika ditampilkan bisa tanpa if else. ada fiturnya. 
     * Jika selain SQL Server disesuaikan.
     * pembuatan Enum dipisah karena bukan representasi folder. Ada class sendiri. 
     * Tambahan directory Utilities - Enums - Buat Class Enum Sesuai yg dibutuhkan
     * Tidak bisa pake spase
     * 
     */
    public enum StatusLevel
    {

        Requested,
        Approved,
        Rejected,
        Canceled,
        Completed, 
        [Display(Name ="On Going")] OnGoing //Enum tidak bisa spasi, Bisa Pake anotasi (Display Name)
    }
}
