using System;
using Xunit;
using NswLicensePlateLookup.Services;
using System.Threading.Tasks;

namespace NswLicensePlateLookupTests
{
    public class PlateLookupServiceTests
    {
        [Fact]
        public async Task GivenEmptyStringPlateNumber_WhenPlateDetailsAreRequested_ThenArgumentExceptionIsThrown()
        {
            // Arrange
            var plateLookupService = new PlateLookupService();
            
            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => plateLookupService.GetPlateDetails(""));
        }

        [Fact]
        public async Task GivenValidPlateNumber_WhenPlateDetailsAreRequested_ThenBasicDetailsAreReturnedAsync()
        {
            // Arrange
            var plateLookupService = new PlateLookupService();
            
            // Act 
            var plateDetails = await plateLookupService.GetPlateDetails("ABC123");

            // Assert
            Assert.Equal("", plateDetails);
        }


    }
}
