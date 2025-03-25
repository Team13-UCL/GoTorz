using System.ComponentModel.DataAnnotations;

namespace GoTorz.Model
{   
    public class Hotels
    {
        [Key]
        public int HotelID { get; set; }
        public string Name { get; set; }        
        public decimal Price { get; set; }

        public DateOnly CheckInDate { get; set; }

        public DateOnly CheckOutDate { get; set; }




    }
}
