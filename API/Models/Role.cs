using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_roles")]
    public class Role : GeneralModel
    {
        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        public ICollection<AccountRole>? AccountRoles { get; set; }  
    }
}
