using Xunit;
using NswLicensePlateLookup.Interfaces;
using System.Threading.Tasks;
using FakeItEasy;
using NswLicensePlateLookup.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace NswLicensePlateLookupTests
{
    public class PlateLookupControllerTests
    {
        private PlateLookupController _plateLookupController;

        private IPlateLookupService _fakePlateLookupService;

        public PlateLookupControllerTests()
        {
            _fakePlateLookupService = A.Fake<IPlateLookupService>();
            _plateLookupController = new PlateLookupController(_fakePlateLookupService);
        }

        [Fact]
        public async Task GivenValidPlateNumber_WhenPlateDetailsAreRequested_ThenBasicDetailsAreReturned()
        {
            // Arrange
            var plateNumber = "RWAGON";
            var expectedPlateDetails = "GOLF";
            A.CallTo(() => _fakePlateLookupService.GetPlateDetails(plateNumber)).Returns(expectedPlateDetails);

            // Act 
            var plateDetailsResponse = await _plateLookupController.GetPlateDetails(plateNumber);

            // Assert
            var plateDetails = plateDetailsResponse.Result as OkObjectResult;;
            Assert.Equal(expectedPlateDetails, plateDetails.Value);
        }
    }
}
