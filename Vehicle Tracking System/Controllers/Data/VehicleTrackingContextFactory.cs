using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Vehicle_Tracking_System.Controllers.Data
{
    public class VehicleTrackingContextFactory : IDesignTimeDbContextFactory<VehicleTrackingContext>
    {
        public VehicleTrackingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VehicleTrackingContext>();

            // Build configuration from appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new VehicleTrackingContext(optionsBuilder.Options);
        }
    }
}
