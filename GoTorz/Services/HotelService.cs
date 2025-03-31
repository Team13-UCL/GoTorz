using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml;
using System.Text.Json.Nodes;


namespace GoTorz.Services
{
    public class HotelService
    {
        private readonly HttpClient _httpClient;

        public HotelService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetExchangeRatesAsync()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com.p.rapidapi.com/v1/hotels/reviews?page_number=0&language_filter=en-gb%2Cde%2Cfr&hotel_id=1676161&locale=en-gb&sort_type=SORT_MOST_RELEVANT&customer_type=solo_traveller%2Creview_category_group_of_friends"),
                Headers =
                {
                    { "x-rapidapi-key", "c5c4e17c1bmsh71eb5409c03590ap1ffd1ajsnaaf196f0baba" },
                    { "x-rapidapi-host", "booking-com.p.rapidapi.com" }
                }
            };

            var response = await _httpClient.SendAsync(request);
            string json = await response.Content.ReadAsStringAsync();
            JsonNode node = JsonNode.Parse(json);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
