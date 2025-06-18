using Microsoft.AspNetCore.Mvc;
using Vehicle_Tracking_System.Controllers.Data;
using Vehicle_Tracking_System.Services;

namespace Vehicle_Tracking_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        /// <summary>
        /// Get all vehicles
        /// </summary>
        /// <returns>List of all vehicles</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicles>>> GetAllVehicles()
        {
            try
            {
                var vehicles = await _vehicleService.GetAllVehiclesAsync();
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get a specific vehicle by vehicle number
        /// </summary>
        /// <param name="vehicleNumber">The vehicle number to search for</param>
        /// <returns>Vehicle details</returns>
        [HttpGet("{vehicleNumber}")]
        public async Task<ActionResult<Vehicles>> GetVehicle(string vehicleNumber)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleByNumberAsync(vehicleNumber);
                if (vehicle == null)
                    return NotFound($"Vehicle with number {vehicleNumber} not found");

                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Create a new vehicle
        /// </summary>
        /// <param name="vehicle">Vehicle data to create</param>
        /// <returns>Created vehicle</returns>
        [HttpPost]
        public async Task<ActionResult<Vehicles>> CreateVehicle([FromBody] Vehicles vehicle)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdVehicle = await _vehicleService.CreateVehicleAsync(vehicle);
                return CreatedAtAction(nameof(GetVehicle),
                    new { vehicleNumber = createdVehicle.VehicleNumber }, createdVehicle);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing vehicle
        /// </summary>
        /// <param name="vehicleNumber">Vehicle number to update</param>
        /// <param name="vehicle">Updated vehicle data</param>
        /// <returns>No content if successful</returns>
        [HttpPut("{vehicleNumber}")]
        public async Task<IActionResult> UpdateVehicle(string vehicleNumber, [FromBody] Vehicles vehicle)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _vehicleService.UpdateVehicleAsync(vehicleNumber, vehicle);
                if (!result)
                    return NotFound($"Vehicle with number {vehicleNumber} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a vehicle
        /// </summary>
        /// <param name="vehicleNumber">Vehicle number to delete</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{vehicleNumber}")]
        public async Task<IActionResult> DeleteVehicle(string vehicleNumber)
        {
            try
            {
                var result = await _vehicleService.DeleteVehicleAsync(vehicleNumber);
                if (!result)
                    return NotFound($"Vehicle with number {vehicleNumber} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Check if a vehicle exists
        /// </summary>
        /// <param name="vehicleNumber">Vehicle number to check</param>
        /// <returns>200 if exists, 404 if not found</returns>
        [HttpHead("{vehicleNumber}")]
        public async Task<IActionResult> CheckVehicleExists(string vehicleNumber)
        {
            try
            {
                var exists = await _vehicleService.VehicleExistsAsync(vehicleNumber);
                return exists ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

