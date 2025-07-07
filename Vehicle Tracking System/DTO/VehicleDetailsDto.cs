namespace Vehicle_Tracking_System.DTO
{
    public class VehicleDetailsDto
    {
        public int VehicleId { get; set; }
        public string VehicleNumber { get; set; }
        public int Capacity { get; set; }
        public string FuelType { get; set; }
        public string CurrentStatus { get; set; }
        public string CurrentLocation { get; set; }
        public DateTime LastUpdated { get; set; }

        public OwnerDto Owner { get; set; }
        public List<DriverDto> Drivers { get; set; } = new List<DriverDto>();
       
        public List<TrackingHistoryDto> RecentTrackingHistory { get; set; } = new List<TrackingHistoryDto>();
    }
}
