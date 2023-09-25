namespace API.Models
{
    public class AccountRole : GeneralModel
    {
        public Guid AccountGuid {  get; set; }
        public Guid RoleGuid { get; set; }
    }
}
