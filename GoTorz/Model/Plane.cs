namespace GoTorz.Model
{
    public class Plane
    {
        public int PlaneId { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public string CarrierCode { get; set; }
        public decimal Price { get; set; }
    }
}
