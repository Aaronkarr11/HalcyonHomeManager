using SQLite;

namespace HalcyonHomeManager.Entities
{
    public class RequestItems
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string DeviceName { get; set; }
        public string Title { get; set; }
        public string ReasonDescription { get; set; }
        public DateTime? DesiredDate { get; set; }
        public string DesiredDateDisplay { get; set; }
    }
}
