using Vehicle_Tracking_System.DTO;

namespace Vehicle_Tracking_System.Services

{
    public interface IVehicleTrackingService
    {
        Task<bool> UpdateVehicleArrivalAsync(int vehicleId, string location, string recordedBy);
        Task<List<VehicleArrivalDto>> GetArrivedVehiclesAsync();
        Task<VehicleDetailsDto> GetVehicleDetailsAsync(int vehicleId);
    }
}
