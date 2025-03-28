namespace GoTorz.Model
{
    public class ReturnPlane
    {
        public int ReturnPlaneId { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public string CarrierCode { get; set; }
        public decimal Price { get; set; }
        public DateTime ReturnDate { get; set; }
        public int Duration { get; set; }
    }
}
