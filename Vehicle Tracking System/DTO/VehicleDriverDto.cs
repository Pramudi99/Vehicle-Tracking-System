namespace Vehicle_Tracking_System.DTO
{
    public class VehicleDriverDto
    {
        public DateTime AssignedDate { get; set; }
        public DateTime? UnassignedDate { get; set; }
        public bool IsActive { get; set; }
        public DriverDto Driver { get; set; }
    }
}
