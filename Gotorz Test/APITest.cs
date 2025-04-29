using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GoTorz;
using GoTorz.Components.Pages.Login;
using GoTorz.Components.Pages.PackagePages;
using GoTorz.Services;
using Xunit;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Collections.Generic;

namespace Gotorz_Test
{
    public class APITest
    {
        public class AmadeusAuthServiceTests
        {
        // Tester Amadeus API access token
        [Fact]
        public async Task GetAccessTokenAsync_ReturnsToken_WhenSuccessful()
        {
            // Arrange
            var expectedToken = "test_access_token";

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent("{\"access_token\": \"" + expectedToken + "\", \"token_type\":\"Bearer\", \"expires_in\": 3600}")
               });

            var httpClient = new HttpClient(handlerMock.Object);

            var inMemorySettings = new Dictionary<string, string> {
                {"Amadeus:ApiKey", "fakeKey"},
                {"Amadeus:ApiSecret", "fakeSecret"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var service = new AmadeusAuthService(httpClient, configuration);

            // Act
            var token = await service.GetAccessTokenAsync();

            // Assert
            Assert.Equal(expectedToken, token);
        }

        // Tester Amadeus API flight 
        [Fact]
        public async Task SearchFlightsAsync_ReturnsFlights_WhenSuccessful()
        {
            // Arrange
            var MockFlightData = "{\"data\": [{\"id\": \"F1\", \"originLocationCode\":\"CPH\", \"destinationLocationCode\":\"CDG\"}]}";

                var handlerMock = new Mock<HttpMessageHandler>();
                handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(MockFlightData)
               });

            var httpClient = new HttpClient(handlerMock.Object);

            var inMemorySettings = new Dictionary<string, string> {
            {"Amadeus:ApiKey", "fakeKey"},
            {"Amadeus:ApiSecret", "fakeSecret"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var service = new AmadeusAuthService(httpClient, configuration);

            // Act
            var result = await service.SearchFlightsAsync("CPH", "CDG", "2025-06-01");

            // Assert
            Assert.Contains("\"destinationLocationCode\":\"CDG\"", result);
        }


        // Tester Hotel
        [Fact]
        public async Task SearchHotelsAsync_ReturnsHotels_WhenSuccessful()
        {
            // Arrange
            var MockHotelData = "{\"data\": [{\"id\": \"H1\", \"cityCode\":\"PAR\"}]}";

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(MockHotelData)
               });

            var httpClient = new HttpClient(handlerMock.Object);

            var inMemorySettings = new Dictionary<string, string> {
            {"Amadeus:ApiKey", "fakeKey"},
            {"Amadeus:ApiSecret", "fakeSecret"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var service = new AmadeusAuthService(httpClient, configuration);

            // Act
            var result = await service.SearchHotelsAsync("PAR");

            // Assert
            Assert.Contains("\"cityCode\":\"PAR\"", result);
        }

        }
    }
}