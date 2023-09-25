﻿using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_employees")]
    public class Employee : GeneralModel
    {
        [Column("nik", TypeName= "nchar(6)")] //Kekuarangan TypeName adalah Hardcode (Tidak Terdeteksi Error), kesusahan saat debug
        public string Nik { get; set; }
        [Column("first_name", TypeName = "nvarchar(100)")]
        public string FirstName{ get; set; }
        [Column("last_name", TypeName = "nvarchar(100)")]
        public string? LastName { get; set; }
        [Column("birth_date")]
        public DateTime BirthDate { get; set; }
        [Column("gender")]
        public int Gender {  get; set; }
        [Column("hiring_date")]
        public DateTime HiringDate { get; set; }

        [Column("email", TypeName ="nvarchar(100)")]
        [Index(IsUnique = true)]
        public string Email { get; set; }
        [Column("phone_number", TypeName = "nvarchar(50)")]
        public string PhoneNumber { get; set; }

    }
}
