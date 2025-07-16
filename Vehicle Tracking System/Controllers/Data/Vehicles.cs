using System.ComponentModel.DataAnnotations;

namespace Vehicle_Tracking_System.Controllers.Data
{
    public class Vehicles
    {
        [Key]
        public int VehicleId { get; set; }
        public string VehicleNumber { get; set; }
        public int Capacity { get; set; }
        public string FuelType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        
        public int OwnerId { get; set; } //foreignKey

        // Navigation property to Owner
        public Owner? Owner { get; set; }

        public ICollection<VehicleTracking> VehicleTracking { get; set; }

        // Navigation property to junction table
        public List<VehicleDriver> VehicleDriver { get; set; } = new List<VehicleDriver>();

        // Computed property to get drivers (optional convenience property)
        public List<Driver> Drivers => VehicleDriver?.Select(vd => vd.Driver).ToList() ?? new List<Driver>();
        public VehicleStatus VehicleStatus { get; set; }

    }
}
