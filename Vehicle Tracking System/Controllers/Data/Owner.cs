using System.ComponentModel.DataAnnotations;

namespace Vehicle_Tracking_System.Controllers.Data
{
    public class Owner
    {
        [Key]
        public int OwnerId { get; set; }
        public int VehicleId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerNic {  get; set; }
        public string OwnerPhone { get; set; }

        // Navigation property to vehicles (1:M)
        public List<Vehicles> Vehicle { get; set; } = new List<Vehicles>();
    }
}
