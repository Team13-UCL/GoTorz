using System.Text.Json.Serialization;

namespace GoTorz.Model
{
    public class Flight
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("itineraries")]
        public List<Itinerary> Itineraries { get; set; }

        [JsonPropertyName("price")]
        public Price Price { get; set; }
    }

    public class Itinerary
    {
        [JsonPropertyName("segments")]
        public List<Segment> Segments { get; set; }
    }

    public class Segment
    {
        [JsonPropertyName("departure")]
        public DepartureArrival Departure { get; set; }

        [JsonPropertyName("arrival")]
        public DepartureArrival Arrival { get; set; }

        [JsonPropertyName("carrierCode")]
        public string CarrierCode { get; set; }

        [JsonPropertyName("number")]
        public string FlightNumber { get; set; }
    }

    public class DepartureArrival
    {
        [JsonPropertyName("iataCode")]
        public string IataCode { get; set; }

        [JsonPropertyName("at")]
        public DateTime At { get; set; }
    }

    public class Price
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("total")]
        public string Total { get; set; }
    }
}