namespace HalcyonHomeManager.Models
{
    public class RequestItemsModel
    {
        public string DeviceName { get; set; }
        public string Title { get; set; }
        public string ReasonDescription { get; set; }
        public DateTime? DesiredDate { get; set; }
        public string DesiredDateDisplay { get; set; }
        public int IsFulfilled { get; set; }
    }
}
