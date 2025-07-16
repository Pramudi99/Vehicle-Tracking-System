using Vehicle_Tracking_System.Controllers.Data;
using Vehicle_Tracking_System.DTO;

namespace Vehicle_Tracking_System.Services
{
    public interface IVehicleService
    {
        Task<Vehicles> CreateVehicleAsync(CreateVehicleDto vehicleDto);
        Task<IEnumerable<Vehicles>> GetAllVehiclesAsync();
        Task<Vehicles?> GetVehicleByNumberAsync(string vehicleNumber);
        Task<bool> UpdateVehicleAsync(string vehicleNumber, Vehicles vehicle);
        Task<bool> DeleteVehicleAsync(string vehicleNumber);
        Task<bool> VehicleExistsAsync(string vehicleNumber);
    }
}

