using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_educations")]
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
        [Column("major", TypeName = "nvarchar(100)")]
        public string Major {  get; set; }
        [Column("degree", TypeName = "nvarchar(100)")]
        public string Degree { get; set; }
        [Column("gpa")]
        public float Gpa {  get; set; }
        [Column("university_guid", TypeName = "uniqueidentifier")]
        public Guid UniversityGuid {  get; set; }

        //Cardinality
        public University? University { get; set; }

        public Employee? Employee { get; set; }
    }
}
