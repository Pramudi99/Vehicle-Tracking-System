using System.ComponentModel.DataAnnotations;

namespace Vehicle_Tracking_System.Controllers.Data
{
    public class VehicleTracking
    {
        [Key]
        public int TrackingId { get; set; }
        public int VehicleId { get; set; }
        public string Status { get; set; } // Departed, Arrived, In Transit
        public string Location { get; set; }
        public DateTime Timestamp { get; set; }

        public string RecordedBy { get; set; }


        // Navigation property
        public Vehicles Vehicle { get; set; }
    }
}
