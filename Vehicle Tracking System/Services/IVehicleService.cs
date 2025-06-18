using Vehicle_Tracking_System.Controllers.Data;

namespace Vehicle_Tracking_System.Services
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicles>> GetAllVehiclesAsync();
        Task<Vehicles?> GetVehicleByNumberAsync(string vehicleNumber);
        Task<Vehicles> CreateVehicleAsync(Vehicles vehicle);
        Task<bool> UpdateVehicleAsync(string vehicleNumber, Vehicles vehicle);
        Task<bool> DeleteVehicleAsync(string vehicleNumber);
        Task<bool> VehicleExistsAsync(string vehicleNumber);
    }
}

