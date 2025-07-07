using Microsoft.AspNetCore.Mvc;
using Vehicle_Tracking_System.Services;
using Vehicle_Tracking_System.DTO;
using System.Threading.Tasks;

namespace Vehicle_Tracking_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleTrackingController : ControllerBase
    {
        private readonly IVehicleTrackingService _vehicleTrackingService;

        public VehicleTrackingController(IVehicleTrackingService vehicleTrackingService)
        {
            _vehicleTrackingService = vehicleTrackingService;
        }

        /// <summary>
        /// Update vehicle arrival status
        /// </summary>
        /// <param name="request">Vehicle arrival update request</param>
        /// <returns>Success or failure response</returns>
        [HttpPost("arrival")]
        public async Task<ActionResult> UpdateVehicleArrival([FromBody] VehicleArrivalUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _vehicleTrackingService.UpdateVehicleArrivalAsync(
                    request.VehicleId,
                    request.Location,
                    request.RecordedBy);

                if (result)
                {
                    return Ok(new { Message = "Vehicle arrival updated successfully" });
                }
                else
                {
                    return StatusCode(500, new { Message = "Failed to update vehicle arrival" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating vehicle arrival", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get all arrived vehicles
        /// </summary>
        /// <returns>List of arrived vehicles</returns>
        [HttpGet("arrived")]
        public async Task<ActionResult<List<VehicleArrivalDto>>> GetArrivedVehicles()
        {
            try
            {
                var arrivedVehicles = await _vehicleTrackingService.GetArrivedVehiclesAsync();
                return Ok(arrivedVehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving arrived vehicles", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get detailed information about a specific vehicle
        /// </summary>
        /// <param name="vehicleId">Vehicle ID</param>
        /// <returns>Vehicle details</returns>
        [HttpGet("{vehicleId}")]
        public async Task<ActionResult<VehicleDetailsDto>> GetVehicleDetails(int vehicleId)
        {
            if (vehicleId <= 0)
            {
                return BadRequest(new { Message = "Invalid vehicle ID" });
            }

            try
            {
                var vehicleDetails = await _vehicleTrackingService.GetVehicleDetailsAsync(vehicleId);

                if (vehicleDetails == null)
                {
                    return NotFound(new { Message = $"Vehicle with ID {vehicleId} not found" });
                }

                return Ok(vehicleDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving vehicle details", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get vehicle status summary
        /// </summary>
        /// <returns>Summary of vehicle statuses</returns>
        [HttpGet("status/summary")]
        public async Task<ActionResult> GetVehicleStatusSummary()
        {
            try
            {
                var arrivedVehicles = await _vehicleTrackingService.GetArrivedVehiclesAsync();

                var summary = new
                {
                    TotalArrivedVehicles = arrivedVehicles.Count,
                    ArrivedVehicles = arrivedVehicles.Select(v => new
                    {
                        v.VehicleId,
                        v.VehicleNumber,
                        v.CurrentLocation,
                        v.ArrivedAt
                    }).ToList()
                };

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving vehicle status summary", Error = ex.Message });
            }
        }
    }

    /// <summary>
    /// Request model for updating vehicle arrival
    /// </summary>
    public class VehicleArrivalUpdateRequest
    {
        public int VehicleId { get; set; }
        public string Location { get; set; } = string.Empty;
        public string RecordedBy { get; set; } = string.Empty;
    }
}
