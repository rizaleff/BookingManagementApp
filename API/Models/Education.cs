namespace API.Models
{
    public class Education : GeneralModel
    {
        /*Guid -> Global Unique Identifier
         * Kalo Di Java Uuid
         * Isinya bakal random string yang panjangnya 32 karakter
         * Di C# kalo ada duplicate akan generate baru 
         * Tidak berurutan
         * Digunakan untuk identifier (ex. identifikasi Primary Key)
         * penggunaan Guid sbg primary key lebih aman
         * Di SQL Server disimpan sbg Unique Identifier, Kalo di Mysql akan disimpan sbg varchar dg panjang 30 sekian
         * Int sbg primary key cukup berbahaya. Di JSON perlu id yang ditampilkan. 
         * Dikhawatirkan ketika data terekspose, dan hacker bisa menebak data selanjutnya (Bruteforce).
         */
        public string Major {  get; set; }
        public string Degree { get; set; }
        public float Gpa {  get; set; }
        public Guid UniversityGuid {  get; set; }

    }
}
