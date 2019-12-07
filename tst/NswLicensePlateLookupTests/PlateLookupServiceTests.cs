using System;
using Xunit;
using NswLicensePlateLookup.Services;
using NswLicensePlateLookup.Interfaces;
using System.Threading.Tasks;
using FakeItEasy;
using NswLicensePlateLookup.Models;

namespace NswLicensePlateLookupTests
{
    public class PlateLookupServiceTests
    {
        private IServiceNswRequestHelper _fakeServiceNswRequestHelper;

        private PlateLookupService _plateLookupService;

        public PlateLookupServiceTests()
        {
            _fakeServiceNswRequestHelper = A.Fake<IServiceNswRequestHelper>();
            _plateLookupService = new PlateLookupService(_fakeServiceNswRequestHelper);
        }

        [Fact]
        public async Task GivenEmptyStringPlateNumber_WhenPlateDetailsAreRequested_ThenArgumentExceptionIsThrown()
        {
            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _plateLookupService.GetPlateDetails(""));
        }

        [Fact]
        public async Task GivenValidPlateNumber_WhenPlateDetailsAreRequested_ThenBasicDetailsAreReturned()
        {
            // Arrange
            var plateNumber = "RWAGON";
            A.CallTo(() => _fakeServiceNswRequestHelper.GetPlateDetails(plateNumber)).Returns(SuccessfulPlateDetailsResponseVehicle);

            // Act 
            var plateDetails = await _plateLookupService.GetPlateDetails(plateNumber);

            // Assert
            Assert.Equal(SuccessfulPlateDetailsResponseVehicle, plateDetails);
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
