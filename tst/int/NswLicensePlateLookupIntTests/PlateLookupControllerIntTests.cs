using Xunit;
using NswLicensePlateLookup.Interfaces;
using System.Threading.Tasks;
using NswLicensePlateLookup.Controllers;
using Microsoft.AspNetCore.Mvc;
using NswLicensePlateLookup.Models;

namespace NswLicensePlateLookupTests
{
    public class PlateLookupControllerTests
    {
        private PlateLookupController _plateLookupController;

        private IPlateLookupService _fakePlateLookupService;

        public PlateLookupControllerIntTests()
        {
            _plateLookupController = new PlateLookupController(_fakePlateLookupService);
        }

        [Fact]
        public async Task GivenValidPlateNumber_WhenPlateDetailsAreRequested_ThenPlateDetailsAreReturned()
        {
            // Arrange
            var plateNumber = "RWAGON";
            A.CallTo(() => _fakePlateLookupService.GetPlateDetails(plateNumber)).Returns(SuccessfulPlateDetailsResponseVehicle);

            // Act 
            var plateDetailsResponse = await _plateLookupController.GetPlateDetails(plateNumber);

            // Assert
            var plateDetails = plateDetailsResponse.Result as OkObjectResult;;
            Assert.Equal(SuccessfulPlateDetailsResponseVehicle, plateDetails.Value);
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
    }
}
