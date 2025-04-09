using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GoTorz.Model
{
    public class PackageUser
    {
        // These properties represent the composite foreign key to Package
        public int PackagePlaneId { get; set; }  // Matches Package.PlaneId
        public int PackageHotelId { get; set; }  // Matches Package.HotelId
        public int ReturnPlaneID { get; set; }   // Matches Package.ReturnPlaneID

        public int UserID { get; set; }          // Foreign key to User
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = "Pending";

        // Navigation properties
        public Package Package { get; set; }
        public User User { get; set; }
    }
}
