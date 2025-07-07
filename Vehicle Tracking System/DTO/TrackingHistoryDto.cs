namespace Vehicle_Tracking_System.DTO
{
    public class TrackingHistoryDto
    {
        public string Status { get; set; }
        public string Location { get; set; }
        public DateTime Timestamp { get; set; }
        public string RecordedBy { get; set; }
    }
}
