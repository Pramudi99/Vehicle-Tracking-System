using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

namespace Vehicle_Tracking_System.Controllers.Data
{
    public class VehicleTrackingContext : DbContext
    {
        public VehicleTrackingContext(DbContextOptions<VehicleTrackingContext> options) : base(options)
        { }
        public DbSet<Vehicles> Vehicles { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<VehicleDriver> VehicleDrivers { get; set; }
        public DbSet<VehicleTracking> VehicleTracking { get; set; }
        public DbSet<VehicleStatus> VehicleStatus { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure unique constraints
            modelBuilder.Entity<Vehicles>()
                .HasIndex(v => v.VehicleId)
                .IsUnique();

            modelBuilder.Entity<VehicleTracking>()
                .HasOne(vt => vt.Vehicle)
                .WithMany(v => v.VehicleTracking)
                .HasForeignKey(vt => vt.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

             // Configure VehicleDriver junction table
            modelBuilder.Entity<VehicleDriver>()
                .HasKey(vd => new { vd.VehicleId, vd.DriverId }); // Composite primary key

            // Configure Vehicle -> VehicleDriver relationship (1:M)
            modelBuilder.Entity<VehicleDriver>()
                .HasOne(vd => vd.Vehicle)
                .WithMany(v => v.VehicleDriver)
                .HasForeignKey(vd => vd.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Driver -> VehicleDriver relationship (1:M)
            modelBuilder.Entity<VehicleDriver>()
                .HasOne(vd => vd.Driver)
                .WithMany(d => d.VehicleDriver)
                .HasForeignKey(vd => vd.DriverId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vehicles>()
                .HasOne( v => v.Owner)
                .WithMany( o => o.Vehicle)
                .HasForeignKey(v => v.OwnerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent accidental owner deletion

            modelBuilder.Entity<Vehicles>()
                .HasOne(v => v.VehicleStatus)
                .WithOne(vs => vs.Vehicles)
                .HasForeignKey<VehicleStatus>(vs => vs.VehicleId);

            
             // Unique constraint on VehicleStatus.VehicleId
            modelBuilder.Entity<VehicleStatus>()
                .HasIndex(vs => vs.VehicleId)
                .IsUnique();






        }
    }
}
