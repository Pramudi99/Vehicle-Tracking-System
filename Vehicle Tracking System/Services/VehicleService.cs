using Microsoft.EntityFrameworkCore;
using Vehicle_Tracking_System.Controllers.Data;

namespace Vehicle_Tracking_System.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly VehicleTrackingContext _context;

        public VehicleService(VehicleTrackingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vehicles>> GetAllVehiclesAsync()
        {
            try
            {
                return await _context.Vehicles.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (add logging framework like Serilog/NLog)
                throw new Exception($"Error retrieving vehicles: {ex.Message}", ex);
            }
        }




        public async Task<Vehicles?> GetVehicleByNumberAsync(string vehicleNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(vehicleNumber))
                    return null;

                return await _context.Vehicles.FindAsync(vehicleNumber);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving vehicle {vehicleNumber}: {ex.Message}", ex);
            }
        }




        public async Task<Vehicles> CreateVehicleAsync(Vehicles vehicle)
        {
            try
            {
                if (vehicle == null)
                    throw new ArgumentNullException(nameof(vehicle));

                if (string.IsNullOrWhiteSpace(vehicle.VehicleNumber))
                    throw new ArgumentException("Vehicle number is required");

                if (await VehicleExistsAsync(vehicle.VehicleNumber))
                    throw new InvalidOperationException($"Vehicle with number {vehicle.VehicleNumber} already exists");

                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();
                return vehicle;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating vehicle: {ex.Message}", ex);
            }
        }



        public async Task<bool> UpdateVehicleAsync(string vehicleNumber, Vehicles vehicle)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(vehicleNumber) || vehicle == null)
                    return false;

                var existingVehicle = await _context.Vehicles.FindAsync(vehicleNumber);
                if (existingVehicle == null)
                    return false;

                // Update properties
               
                // Add other properties as needed

                _context.Vehicles.Update(existingVehicle);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating vehicle {vehicleNumber}: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteVehicleAsync(string vehicleNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(vehicleNumber))
                    return false;

                var vehicle = await _context.Vehicles.FindAsync(vehicleNumber);
                if (vehicle == null)
                    return false;

                _context.Vehicles.Remove(vehicle);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting vehicle {vehicleNumber}: {ex.Message}", ex);
            }
        }

        public async Task<bool> VehicleExistsAsync(string vehicleNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(vehicleNumber))
                    return false;

                return await _context.Vehicles.AnyAsync(v => v.VehicleNumber == vehicleNumber);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking vehicle existence: {ex.Message}", ex);
            }
        }
    }
}
