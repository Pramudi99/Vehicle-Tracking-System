using System.ComponentModel.DataAnnotations;

namespace Vehicle_Tracking_System.DTO
{
    public class UpdateVehicleDto
    {
        [Required]
        [StringLength(50)]
        public string VehicleNumber { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }

        [Required]
        [StringLength(50)]
        public string FuelType { get; set; }

        public int? OwnerId { get; set; }
    }
}
