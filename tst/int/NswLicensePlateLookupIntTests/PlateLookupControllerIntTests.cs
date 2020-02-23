using Xunit;
using NswLicensePlateLookup.Interfaces;
using System.Threading.Tasks;
using NswLicensePlateLookup.Controllers;
using NswLicensePlateLookup.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using WireMock.Server;
using WireMock.Settings;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Matchers;
using System.Net.Http;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using FluentAssertions;

namespace NswLicensePlateLookupTests
{
    public class PlateLookupControllerIntTests : IClassFixture<WebApplicationFactory<NswLicensePlateLookup.Startup>>, IDisposable
    {
        private readonly WebApplicationFactory<NswLicensePlateLookup.Startup> _factory;

        private HttpClient _client;

        private WireMockServer _server;

        public PlateLookupControllerIntTests(WebApplicationFactory<NswLicensePlateLookup.Startup> factory)
        {
            InitializeMockServiceNswApi();
            _factory = factory;
            _client = _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context,conf) =>
                    {
                        conf.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Testing.json"));
                    });
                }).CreateClient();
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

            // Act 
            var plateDetailsResponse = await _client.GetAsync($"api/plate/{plateNumber}");

            // Assert
            var plateDetailsString = await plateDetailsResponse.Content.ReadAsStringAsync();
            var plateDetails = JsonConvert.DeserializeObject<PlateDetails>(plateDetailsString);
            plateDetails.Should().BeEquivalentTo(SuccessfulPlateDetailsResponseVehicle);
        }

        private void InitializeMockServiceNswApi()
        {
            _server = WireMockServer.Start(new FluentMockServerSettings
                {
                    Port = 10321,
                }   
            );

            _server
                .Given(
                    Request
                    .Create()
                    .WithPath("/MyServiceNSW/apexremote")
                    .WithBody(new JsonPathMatcher("$..[?(@.method == 'createRMSTransaction')]"))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json;charset=UTF-8")
                    .WithBody(JsonConvert.SerializeObject(SuccessfulTransactionTokenResponse))
                );

            _server
                .Given(
                    Request
                    .Create()
                    .WithPath("/MyServiceNSW/apexremote")
                    .WithBody(new JsonPathMatcher("$..[?(@.method == 'postVehicleListForFreeRegoCheck')]"))
                    .UsingPost())
                .RespondWith(
                    Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json;charset=UTF-8")
                    .WithBody(JsonConvert.SerializeObject(SuccessfulPlateDetailsResponse))
                );
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

        private List<ServiceNswResponse<TokenResult>> SuccessfulTransactionTokenResponse = new List<ServiceNswResponse<TokenResult>>
        {
            new ServiceNswResponse<TokenResult>
            {
                StatusCode = 200,
                Type = "rpc",
                Tid = 1,
                Ref = false,
                Action = "RMSWrapperCtrl",
                Method = "createRMSTransaction",
                Result = new TokenResult 
                    {
                        StatusCode = 2000,
                        StatusMessage = "success",
                        Token = "valid-token"
                    }
            }
        };

        private List<ServiceNswResponse<PlateDetailsResult>> SuccessfulPlateDetailsResponse = new List<ServiceNswResponse<PlateDetailsResult>>
        {
            new ServiceNswResponse<PlateDetailsResult>
            {
                StatusCode = 200,
                Type = "rpc",
                Tid = 1,
                Ref = false,
                Action = "RMSWrapperCtrl",
                Method = "createRMSTransaction",
                Result = new PlateDetailsResult 
                    {
                        StatusCode = 2000,
                        StatusMessage = "success",
                        PlateDetails = new PlateDetails
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
                        }
                    }
            }
        };
    }
}
