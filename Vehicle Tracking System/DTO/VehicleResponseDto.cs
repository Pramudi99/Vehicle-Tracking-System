namespace Vehicle_Tracking_System.DTO
{
    public class VehicleResponseDto
    {
        public string VehicleNumber { get; set; }
        public int Capacity { get; set; }
        public string FuelType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public OwnerDto? Owner { get; set; }
        public List<VehicleDriverDto>? VehicleDriver { get; set; }
    }
}
