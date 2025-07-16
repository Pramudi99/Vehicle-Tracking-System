using System.ComponentModel.DataAnnotations;

namespace Vehicle_Tracking_System.DTO
{
    public class CreateVehicleDto
    {
        [Required]
        [StringLength(50)]
        public string VehicleNumber { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
        public int Capacity { get; set; }

        [Required]
        [StringLength(50)]
        public string FuelType { get; set; }

        public OwnerDto Owner { get; set; }
        public List<VehicleDriverDto> VehicleDriver { get; set; } = new List<VehicleDriverDto>();


    }
}
