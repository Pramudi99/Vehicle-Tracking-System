using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.ComponentModel.DataAnnotations;

namespace Vehicle_Tracking_System.Controllers.Data
{
    public class VehicleStatus
    {
        [Key]
        public int VehicleStatusId { get; set; }
        public int VehicleId { get; set; }

        public string CurrentStatus { get; set; }
        public DateTime LastUpdated {  get; set; }
        public string CurrentLocation { get; set; }

        public Vehicles Vehicles { get; set; }


    }
}
