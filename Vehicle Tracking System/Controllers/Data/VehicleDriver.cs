using System.ComponentModel.DataAnnotations;

namespace Vehicle_Tracking_System.Controllers.Data
{
    public class VehicleDriver
    {
        [Key]
        public int VehicleDriverId { get; set; }
        public int VehicleId { get; set; }
        public int DriverId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? UnassignedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public Vehicles Vehicle { get; set; }
        public Driver Driver { get; set; }
    }
}
