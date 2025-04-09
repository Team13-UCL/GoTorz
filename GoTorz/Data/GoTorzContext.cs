using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GoTorz.Model;
using static GoTorz.Components.Pages.SalesPlanePage;

namespace GoTorz.Data
{
    public class GoTorzContext : DbContext
    {
        public GoTorzContext(DbContextOptions<GoTorzContext> options)
            : base(options)
        {
        }
        //create a composite key
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Package>()
                .HasKey(p => new { p.PlaneId, p.HotelId, p.ReturnPlaneID });

            modelBuilder.Entity<Package>()
                .HasOne(p => p.Plane)
                .WithMany()
                .HasForeignKey(p => p.PlaneId);

            modelBuilder.Entity<Package>()
                .HasOne(p => p.ReturnPlane)
                .WithMany()
                .HasForeignKey(p => p.ReturnPlaneID);

            modelBuilder.Entity<Package>()
                .HasOne(p => p.Hotels)
                .WithMany()
                .HasForeignKey(p => p.HotelId);



            // Configure the composite primary key for PackageUser
            modelBuilder.Entity<PackageUser>()
                .HasKey(pu => new { pu.PackagePlaneId, pu.PackageHotelId, pu.ReturnPlaneID, pu.UserID });

            // Configure the relationship without enforcing database-level foreign key
            modelBuilder.Entity<PackageUser>()
                .HasOne(pu => pu.Package)
                .WithMany()
                .HasPrincipalKey(p => new { p.PlaneId, p.HotelId, p.ReturnPlaneID })
                .HasForeignKey(pu => new { pu.PackagePlaneId, pu.PackageHotelId, pu.ReturnPlaneID })
                .OnDelete(DeleteBehavior.ClientNoAction); // Handle deletion in code


            base.OnModelCreating(modelBuilder);
        }

        //add the DbSet, mapping the model to the database
        public DbSet<Package> Package { get; set; } = default!;
        public DbSet<Plane> Plane { get; set; } = default!;
        public DbSet<Hotels> Hotels { get; set; } = default!;
        public DbSet<ReturnPlane> ReturnPlane { get; set; } = default!;
        public DbSet<User> User { get; set; } = default!;
        public DbSet<PackageUser> PackageUser { get; set; } = default!;
    }
}
