namespace API.Models
{
    public class Room : GeneralModel
    {
        public string Name { get; set; }
        public int Floor { get; set; }
        public int Capacity { get; set; }

    }
}
