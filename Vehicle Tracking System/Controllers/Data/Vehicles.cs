using System.ComponentModel.DataAnnotations;

namespace Vehicle_Tracking_System.Controllers.Data
{
    public class Vehicles
    {
        [Key]
        public string VehicleNumber { get; set; }
        public string DriverName { get; set; }

    }
}
