namespace Vehicle_Tracking_System.DTO
{
    public class VehicleArrivalDto
    {
        public int VehicleId { get; set; }
        public string VehicleNumber { get; set; }
        public int Capacity { get; set; }
        public string FuelType { get; set; }
        public string CurrentLocation { get; set; }
        public DateTime ArrivedAt { get; set; }
        public List<DriverDto> Drivers { get; set; } = new List<DriverDto>();
        public List<OwnerDto> Owners { get; set; } = new List<OwnerDto>();
    }
}
