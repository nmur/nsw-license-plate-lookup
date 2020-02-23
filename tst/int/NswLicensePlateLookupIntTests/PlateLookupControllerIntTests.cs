using Xunit;
using NswLicensePlateLookup.Interfaces;
using System.Threading.Tasks;
using NswLicensePlateLookup.Controllers;
using NswLicensePlateLookup.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using WireMock.Server;
using WireMock.Settings;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using FluentAssertions;

namespace NswLicensePlateLookupTests
{
    public class PlateLookupControllerIntTests : IClassFixture<TestingWebApplicationFactory<NswLicensePlateLookup.Program>>, IDisposable
    {
        private readonly TestingWebApplicationFactory<NswLicensePlateLookup.Program> _factory;

        private HttpClient _client;

        private WireMockServer _server;

        public PlateLookupControllerIntTests(TestingWebApplicationFactory<NswLicensePlateLookup.Program> factory)
        {
            _factory = factory;
            System.Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
            _client = _factory.CreateClient();
            _server = WireMockServer.Start(new FluentMockServerSettings
                {
                    Port = 10321,
                }   
            );
        }

        public void Dispose()
        {
            _server.Stop();
        }

        [Fact]
        public async Task GivenValidPlateNumber_WhenPlateDetailsAreRequested_ThenPlateDetailsAreReturned()
        {
            // Arrange
            var plateNumber = "RWAGON";
            
            _server.Given(Request.Create().WithPath("/MyServiceNSW/apexremote").UsingPost())
                .RespondWith(
                    Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json;charset=UTF-8")
                    .WithBody("{\"Value\": \"Hello world!\"}")
                );

            // Act 
            var plateDetailsResponse = await _client.GetAsync($"api/plate/{plateNumber}");

            // Assert
            var plateDetailsString = await plateDetailsResponse.Content.ReadAsStringAsync();
            var plateDetails = JsonConvert.DeserializeObject<PlateDetails>(plateDetailsString);
            plateDetails.Should().BeEquivalentTo(SuccessfulPlateDetailsResponseVehicle);
        }

        private PlateDetails SuccessfulPlateDetailsResponseVehicle = new PlateDetails
        {
            Vehicle = new Vehicle
            {
                BodyShape = "STATION WAGON",
                Manufacturer = "VLK",
                ManufactureYear = 2015,
                Model = "GOLF",
                NswPlateNumber = "RWAGON",
                PlateType = "O",
                TareWeight = 1509,
                Variant = "16 2.0 6SPA R WE 26KW WAGON",
                VehicleColour = "GREY",
                VehicleType = "PASSENGER VEHICLES",
                VinNumber = "xxxxxxxxxxxxx0823"
            }
        };

        private TokenResult SuccessfulTransactionTokenResponse = new TokenResult
        {
            StatusCode = 2000,
            StatusMessage = "success",
            Token = "valid-token"
        };
    }
}
