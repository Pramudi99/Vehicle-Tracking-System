using Microsoft.AspNetCore.Mvc;
using Vehicle_Tracking_System.DTO;
using Vehicle_Tracking_System.Services;
using Vehicle_Tracking_System.Controllers.Data;

namespace Vehicle_Tracking_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleResponseDto>>> GetAllVehicles()
        {
            try
            {
                var vehicles = await _vehicleService.GetAllVehiclesAsync();
                var vehicleDtos = vehicles.Select(MapToResponseDto).ToList();
                return Ok(vehicleDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving vehicles", details = ex.Message });
            }
        }

        [HttpGet("{vehicleNumber}")]
        public async Task<ActionResult<VehicleResponseDto>> GetVehicle(string vehicleNumber)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleByNumberAsync(vehicleNumber);
                if (vehicle == null)
                {
                    return NotFound(new { error = $"Vehicle with number {vehicleNumber} not found" });
                }

                var vehicleDto = MapToResponseDto(vehicle);
                return Ok(vehicleDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving the vehicle", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<VehicleResponseDto>> CreateVehicle([FromBody] CreateVehicleDto createVehicleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if vehicle already exists
                var existingVehicle = await _vehicleService.VehicleExistsAsync(createVehicleDto.VehicleNumber);
                if (existingVehicle)
                {
                    return Conflict(new { error = "Vehicle already exists", details = $"Vehicle with number {createVehicleDto.VehicleNumber} already exists" });
                }

                // Pass the DTO directly to the service as expected
                var createdVehicle = await _vehicleService.CreateVehicleAsync(createVehicleDto);
                var responseDto = MapToResponseDto(createdVehicle);

                return CreatedAtAction(nameof(GetVehicle), new { vehicleNumber = createdVehicle.VehicleNumber }, responseDto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { error = "Invalid vehicle data", details = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Validation failed", details = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = "Vehicle already exists", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while creating the vehicle", details = ex.Message });
            }
        }

        [HttpPut("{vehicleNumber}")]
        public async Task<ActionResult> UpdateVehicle(string vehicleNumber, [FromBody] UpdateVehicleDto updateVehicleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if vehicle exists before updating
                var vehicleExists = await _vehicleService.VehicleExistsAsync(vehicleNumber);
                if (!vehicleExists)
                {
                    return NotFound(new { error = $"Vehicle with number {vehicleNumber} not found" });
                }

                var vehicle = MapToEntity(updateVehicleDto);
                var success = await _vehicleService.UpdateVehicleAsync(vehicleNumber, vehicle);

                if (!success)
                {
                    return NotFound(new { error = $"Vehicle with number {vehicleNumber} not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while updating the vehicle", details = ex.Message });
            }
        }

        [HttpDelete("{vehicleNumber}")]
        public async Task<ActionResult> DeleteVehicle(string vehicleNumber)
        {
            try
            {
                var success = await _vehicleService.DeleteVehicleAsync(vehicleNumber);
                if (!success)
                {
                    return NotFound(new { error = $"Vehicle with number {vehicleNumber} not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while deleting the vehicle", details = ex.Message });
            }
        }

        [HttpGet("exists/{vehicleNumber}")]
        public async Task<ActionResult<bool>> VehicleExists(string vehicleNumber)
        {
            try
            {
                var exists = await _vehicleService.VehicleExistsAsync(vehicleNumber);
                return Ok(new { exists });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while checking vehicle existence", details = ex.Message });
            }
        }

        // Mapping methods
        private VehicleResponseDto MapToResponseDto(Vehicles vehicle)
        {
            return new VehicleResponseDto
            {
                VehicleNumber = vehicle.VehicleNumber,
                Capacity = vehicle.Capacity,
                FuelType = vehicle.FuelType,
                CreatedDate = vehicle.CreatedDate,
                UpdatedDate = vehicle.UpdatedDate,
                Owner = vehicle.Owner != null ? new OwnerDto
                {
                    OwnerName = vehicle.Owner.OwnerName,
                    OwnerNic = vehicle.Owner.OwnerNic,
                    OwnerPhone = vehicle.Owner.OwnerPhone
                } : null,
                VehicleDriver = vehicle.VehicleDriver?.Select(vd => new VehicleDriverDto
                {
                    AssignedDate = vd.AssignedDate,
                    UnassignedDate = vd.UnassignedDate,
                    IsActive = vd.IsActive,
                    Driver = vd.Driver != null ? new DriverDto
                    {
                        DriverName = vd.Driver.DriverName,
                        DriverPhoto = vd.Driver.DriverPhoto,
                        DriverNic = vd.Driver.DriverNic,
                        DriverPhone = vd.Driver.DriverPhone
                    } : null
                }).ToList()
            };
        }

        private Vehicles MapToEntity(UpdateVehicleDto dto)
        {
            return new Vehicles
            {
                VehicleNumber = dto.VehicleNumber,
                Capacity = dto.Capacity,
                FuelType = dto.FuelType,
                OwnerId = dto.OwnerId ?? 0,
                UpdatedDate = DateTime.UtcNow
            };
        }
    }
}