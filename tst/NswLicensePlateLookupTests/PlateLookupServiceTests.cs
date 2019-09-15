using System;
using Xunit;
using NswLicensePlateLookup.Services;
using NswLicensePlateLookup.Interfaces;
using System.Threading.Tasks;
using FakeItEasy;

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
            var expectedPlateDetails = "GOLF";
            A.CallTo(() => _fakeServiceNswRequestHelper.GetPlateDetails(plateNumber)).Returns(expectedPlateDetails);

            // Act 
            var plateDetails = await _plateLookupService.GetPlateDetails(plateNumber);

            // Assert
            Assert.Equal(expectedPlateDetails, plateDetails);
        }
    }
}
