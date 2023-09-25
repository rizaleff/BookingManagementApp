﻿using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_tr_bookings")]
    public class Booking : GeneralModel
    {
        [Column("start_date")]
        public DateTime StartDate { get; set; }
        [Column("end_date")]
        public DateTime EndDate { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("remarks", TypeName = "nvarchar(max)")]
        public string Remarks { get; set; }
        [Column("room_guid", TypeName ="uniqueidentifier")]
        public Guid RoomGuid { get; set; }
        [Column("employee_guid", TypeName = "uniqueidentifier")]
        public Guid EmployeeGuid { get; set; }
    }
}
