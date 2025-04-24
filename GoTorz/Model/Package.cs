using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoTorz.Model
{
    public class Package
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Fortæller Entity Framework at databasen selv skal generere PackageID-værdier
        public int PackageID { get; set; } // PK
        public int PlaneId { get; set; }
        public int HotelId { get; set; }
        public int ReturnPlaneID { get; set; }

        public decimal? PackagePrice { get; set; }

        [ForeignKey("PlaneId")]
        public Plane Plane { get; set; } = default!;

        [ForeignKey("ReturnPlaneID")]
        public ReturnPlane ReturnPlane { get; set; } = default!;

        [ForeignKey("HotelId")]
        public Hotels Hotels { get; set; } = default!;
    }
}
