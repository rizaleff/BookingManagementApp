namespace API.Models
{
    public abstract class GeneralModel
    {
        public Guid guid { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
