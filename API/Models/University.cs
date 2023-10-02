using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_universities")]
    public class University : GeneralModel
    {
        //Tipe data numeric atau selain string tidak perlu menentukan type name nya
        [Column("code", TypeName = "nvarchar(50)")]
        public string Code { get; set; }
        [Column("name", TypeName = "nvarchar(100)")] //Bisa juga menggunakan MaxLength 
        public string Name { get; set; }

        //Cardinality
        /* Problem nullable ketika insert data
         */
        public ICollection<Education>? Educations { get; set; }


    }
}
