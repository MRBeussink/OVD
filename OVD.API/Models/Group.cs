namespace OVD.API.Models
{
    public class Group
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Hotspares { get; set; }
        public int Total { get; set; }
        public int Online { get; set; }
        public int Active { get; set; }
        public string Image { get; set; }
    }
}