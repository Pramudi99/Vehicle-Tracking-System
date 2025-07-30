using Vehicle_Tracking_System.Controllers.Data;
using Vehicle_Tracking_System.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;



namespace Vehicle_Tracking_System.Services
{
    public class VehicleTrackingService : IVehicleTrackingService
    {
        private readonly VehicleTrackingContext _context;

        public VehicleTrackingService(VehicleTrackingContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateVehicleArrivalAsync(int vehicleId, string location, string recordedBy)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Add tracking record
                var tracking = new VehicleTracking
                {
                    VehicleId = vehicleId,
                    Status = "Arrived",
                    Location = location,
                    Timestamp = DateTime.Now,
                    RecordedBy = recordedBy
                };

                _context.VehicleTracking.Add(tracking);

                // Update current status
                var currentStatus = await _context.VehicleStatus
                    .FirstOrDefaultAsync(vs => vs.VehicleId == vehicleId);

                if (currentStatus == null)
                {
                    currentStatus = new VehicleStatus
                    {
                        VehicleId = vehicleId,
                        CurrentStatus = "Arrived",
                        LastUpdated = DateTime.Now,
                        CurrentLocation = location
                    };
                    _context.VehicleStatus.Add(currentStatus);
                }
                else
                {
                    currentStatus.CurrentStatus = "Arrived";
                    currentStatus.LastUpdated = DateTime.Now;
                    currentStatus.CurrentLocation = location;
                    _context.VehicleStatus.Update(currentStatus);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<List<VehicleArrivalDto>> GetArrivedVehiclesAsync()
        {
            return await _context.VehicleStatus
                .Where(vs => vs.CurrentStatus == "Arrived")
                .Include(vs => vs.Vehicles)
                    .ThenInclude(v => v.VehicleDriver)
                    .ThenInclude(vd => vd.Driver)
                .Select(vs => new VehicleArrivalDto
                {
                    VehicleId = vs.VehicleId,
                    VehicleNumber = vs.Vehicles.VehicleNumber,
                    Capacity = vs.Vehicles.Capacity,
                    FuelType = vs.Vehicles.FuelType,
                    CurrentLocation = vs.CurrentLocation,
                    ArrivedAt = vs.LastUpdated,
                    Drivers = vs.Vehicles.VehicleDriver
                        .Where(vd => vd.IsActive)
                        .Select(vd => new DriverDto
                        {
                            DriverName = vd.Driver.DriverName,
                            DriverPhone = vd.Driver.DriverPhone,
                            DriverPhoto = vd.Driver.DriverPhoto
                        }).ToList(),
                   
                })
                .ToListAsync();
        }

        public async Task<VehicleDetailsDto> GetVehicleDetailsAsync(int vehicleId)
        {
            return await _context.Vehicles
                .Where(v => v.VehicleId == vehicleId)
                .Include(v => v.VehicleDriver)
                    .ThenInclude(vd => vd.Driver)
               
                .Include(v => v.VehicleStatus)
                .Include(v => v.VehicleTracking.OrderByDescending(vt => vt.Timestamp).Take(10))
                .Select(v => new VehicleDetailsDto
                {
                    VehicleId = v.VehicleId,
                    VehicleNumber = v.VehicleNumber,
                    Capacity = v.Capacity,
                    FuelType = v.FuelType,
                    CurrentLocation = v.VehicleStatus.CurrentLocation,
                    LastUpdated = v.VehicleStatus.LastUpdated,
                    Drivers = v.VehicleDriver
                        .Where(vd => vd.IsActive)
                        .Select(vd => new DriverDto
                        {
                            DriverName = vd.Driver.DriverName,
                            DriverPhone = vd.Driver.DriverPhone,
                            DriverPhoto = vd.Driver.DriverPhoto,
                            DriverNic = vd.Driver.DriverNic
                        }).ToList(),
                    RecentTrackingHistory = v.VehicleTracking
                        .Select(vt => new TrackingHistoryDto
                        {
                            Status = vt.Status,
                            Location = vt.Location,
                            Timestamp = vt.Timestamp,
                            RecordedBy = vt.RecordedBy
                        }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}
