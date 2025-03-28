using System.Text.Json.Serialization;

namespace GoTorz.Model
{
    // Model classes
    public class HotelResponse
    {
        [JsonPropertyName("data")]
        public List<Hotel> Data { get; set; }
    }

    public class Hotel
    {
        [JsonPropertyName("hotelId")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("iataCode")]
        public string IataCode { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }
    }

    public class Address
    {
        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }
    }

    // Hotel Offers Models
    public class HotelOfferResponse
    {
        [JsonPropertyName("data")]
        public List<HotelOfferData> Data { get; set; }
    }

    public class HotelOfferData
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("hotel")]
        public HotelInfo Hotel { get; set; }

        [JsonPropertyName("available")]
        public bool Available { get; set; }

        [JsonPropertyName("offers")]
        public List<Offer> Offers { get; set; }
    }

    public class HotelInfo
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("hotelId")]
        public string HotelId { get; set; }

        [JsonPropertyName("chainCode")]
        public string ChainCode { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("cityCode")]
        public string CityCode { get; set; }

        [JsonPropertyName("address")]
        public HotelAddress Address { get; set; }
    }

    public class HotelAddress
    {
        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }
    }

    public class Offer
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("checkInDate")]
        public string CheckInDate { get; set; }

        [JsonPropertyName("checkOutDate")]
        public string CheckOutDate { get; set; }

        [JsonPropertyName("rateCode")]
        public string RateCode { get; set; }

        [JsonPropertyName("room")]
        public Room Room { get; set; }

        [JsonPropertyName("guests")]
        public Guests Guests { get; set; }

        [JsonPropertyName("price")]
        public Price Price { get; set; }
    }

    public class Room
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("typeEstimated")]
        public TypeEstimated TypeEstimated { get; set; }

        [JsonPropertyName("description")]
        public Description Description { get; set; }
    }

    public class TypeEstimated
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("beds")]
        public int? Beds { get; set; }

        [JsonPropertyName("bedType")]
        public string BedType { get; set; }
    }

    public class Description
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class Guests
    {
        [JsonPropertyName("adults")]
        public int Adults { get; set; }
    }

    public class HotelPrice
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("total")]
        public string Total { get; set; }

        [JsonPropertyName("base")]
        public string Base { get; set; }

        [JsonPropertyName("taxes")]
        public List<Tax> Taxes { get; set; }
    }

    public class Tax
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}
