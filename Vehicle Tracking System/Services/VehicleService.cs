using Microsoft.EntityFrameworkCore;
using Vehicle_Tracking_System.Controllers.Data;
using Vehicle_Tracking_System.DTO;

namespace Vehicle_Tracking_System.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly VehicleTrackingContext _context;

        public VehicleService(VehicleTrackingContext context)
        {
            _context = context;
        }

        public async Task<Vehicles> CreateVehicleAsync(CreateVehicleDto vehicleDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Owner owner;

                // Check if owner already exists by NIC (unique identifier)
                var existingOwner = await _context.Owners
                    .FirstOrDefaultAsync(o => o.OwnerNic == vehicleDto.Owner.OwnerNic);

                if (existingOwner != null)
                {
                    // Use existing owner
                    owner = existingOwner;
                }
                else
                {
                    // Create new owner
                    owner = new Owner
                    {
                        OwnerName = vehicleDto.Owner.OwnerName,
                        OwnerNic = vehicleDto.Owner.OwnerNic,
                        OwnerPhone = vehicleDto.Owner.OwnerPhone
                    };

                    _context.Owners.Add(owner);
                    await _context.SaveChangesAsync(); // Save to get the OwnerId
                }

                // Create the vehicle
                var vehicle = new Vehicles
                {
                    VehicleNumber = vehicleDto.VehicleNumber,
                    Capacity = vehicleDto.Capacity,
                    FuelType = vehicleDto.FuelType,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    OwnerId = owner.OwnerId.Value, // Use the owner's ID
                    VehicleDriver = new List<VehicleDriver>()
                };

                // Handle drivers if provided
                if (vehicleDto.VehicleDriver?.Any() == true)
                {
                    foreach (var driverDto in vehicleDto.VehicleDriver)
                    {
                        Driver driver;

                        // Check if driver already exists by NIC
                        var existingDriver = await _context.Drivers
                            .FirstOrDefaultAsync(d => d.DriverNic == driverDto.Driver.DriverNic);

                        if (existingDriver != null)
                        {
                            driver = existingDriver;
                        }
                        else
                        {
                            // Create new driver
                            driver = new Driver
                            {
                                DriverName = driverDto.Driver.DriverName,
                                DriverNic = driverDto.Driver.DriverNic,
                                DriverPhone = driverDto.Driver.DriverPhone,
                                DriverPhoto = driverDto.Driver.DriverPhoto
                            };

                            _context.Drivers.Add(driver);
                            await _context.SaveChangesAsync(); // Save to get the DriverId
                        }

                        // Create the vehicle-driver relationship
                        var vehicleDriver = new VehicleDriver
                        {
                            Vehicle = vehicle,
                            Driver = driver,
                            AssignedDate = driverDto.AssignedDate,
                            UnassignedDate = driverDto.UnassignedDate,
                            IsActive = driverDto.IsActive
                        };

                        vehicle.VehicleDriver.Add(vehicleDriver);
                    }
                }

                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                // Return the created vehicle with all related data
                return await GetVehicleByIdAsync(vehicle.VehicleId);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error creating vehicle: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Vehicles>> GetAllVehiclesAsync()
        {
            try
            {
                return await _context.Vehicles
                   .Include(v => v.Owner)
                   .Include(v => v.VehicleDriver)
                       .ThenInclude(vd => vd.Driver)
                   .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving vehicles: {ex.Message}", ex);
            }
        }

        public async Task<Vehicles?> GetVehicleByNumberAsync(string vehicleNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(vehicleNumber))
                    return null;

                return await _context.Vehicles
                                    .Include(v => v.Owner)
                                    .Include(v => v.VehicleDriver)
                                        .ThenInclude(vd => vd.Driver)
                                    .FirstOrDefaultAsync(v => v.VehicleNumber == vehicleNumber);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving vehicle {vehicleNumber}: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateVehicleAsync(string vehicleNumber, Vehicles vehicle)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(vehicleNumber) || vehicle == null)
                    return false;

                var existingVehicle = await _context.Vehicles
                    .FirstOrDefaultAsync(v => v.VehicleNumber == vehicleNumber);
                if (existingVehicle == null)
                    return false;

                // Update only the allowed properties
                existingVehicle.VehicleNumber = vehicle.VehicleNumber;
                existingVehicle.Capacity = vehicle.Capacity;
                existingVehicle.FuelType = vehicle.FuelType;
                existingVehicle.UpdatedDate = DateTime.UtcNow;
                existingVehicle.OwnerId = vehicle.OwnerId;

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

                var vehicle = await _context.Vehicles
                    .FirstOrDefaultAsync(v => v.VehicleNumber == vehicleNumber);
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

        private async Task<Vehicles> GetVehicleByIdAsync(int vehicleId)
        {
            return await _context.Vehicles
                .Include(v => v.Owner)
                .Include(v => v.VehicleDriver)
                    .ThenInclude(vd => vd.Driver)
                .FirstOrDefaultAsync(v => v.VehicleId == vehicleId);
        }
    }
}