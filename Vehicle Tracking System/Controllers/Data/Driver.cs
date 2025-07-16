using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vehicle_Tracking_System.Controllers.Data
{
    public class Driver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DriverId {  get; set; }
        public string DriverName { get; set; }
        public string DriverPhoto {  get; set; }
        public string DriverNic { get; set; }
        public string DriverPhone { get; set; }

        // Navigation property to junction table
        public List<VehicleDriver> VehicleDriver { get; set; } = new List<VehicleDriver>();

        // Computed property to get vehicles (optional convenience property)
        public List<Vehicles> Vehicles => VehicleDriver?.Select(vd => vd.Vehicle).ToList() ?? new List<Vehicles>();
    }
}
