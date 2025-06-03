using Microsoft.EntityFrameworkCore;

namespace Vehicle_Tracking_System.Controllers.Data
{
    public class VehicleTrackingContext : DbContext
    {
        public VehicleTrackingContext(DbContextOptions<VehicleTrackingContext> options) : base(options)
        { }
        public DbSet<Vehicles> Vehicles { get; set; }
    }
}
