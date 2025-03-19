using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GoTorz.Model;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;



public class AmadeusAuthService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "EmYetVmoFewLD2DngcYFq9RakHNBNUKl";
    private readonly string _apiSecret = "nP5QKb8W4ioDR21h";
    private readonly string _tokenUrl = "https://test.api.amadeus.com/v1/security/oauth2/token";

    private string _accessToken;
    private DateTime _tokenExpiration;

    public AmadeusAuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        if (!string.IsNullOrEmpty(_accessToken) && _tokenExpiration > DateTime.UtcNow)
        {
            return _accessToken;
        }

        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", _apiKey },
            { "client_secret", _apiSecret }
        };

        var content = new FormUrlEncodedContent(parameters);
        var response = await _httpClient.PostAsync(_tokenUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AmadeusTokenResponse>(jsonResponse);
            _accessToken = result.AccessToken;
            _tokenExpiration = DateTime.UtcNow.AddSeconds(result.ExpiresIn);
            return _accessToken;
        }
        else
        {
            throw new HttpRequestException($"Failed to get access token: {response.ReasonPhrase}");
        }
    }

    public async Task<string> SearchFlightsAsync(string origin, string destination, string date)
    {
        string accessToken = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        string apiUrl = $"https://test.api.amadeus.com/v2/shopping/flight-offers?originLocationCode={origin}&destinationLocationCode={destination}&departureDate={date}&adults=1";

        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new HttpRequestException($"Failed to fetch flights: {response.ReasonPhrase}");
        }
    }
    public async Task<string> SearchHotelsAsync(string cityCode)
    {
        string accessToken = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        string apiUrl = $"https://test.api.amadeus.com/v1/reference-data/locations/hotels/by-city?cityCode={cityCode}";

        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new HttpRequestException($"Failed to fetch hotels: {response.ReasonPhrase}");
        }
    }

}



public class AmadeusTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}
