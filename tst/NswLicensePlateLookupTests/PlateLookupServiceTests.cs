using System;
using Xunit;
using NswLicensePlateLookup.Services;

namespace NswLicensePlateLookupTests
{
    public class PlateLookupServiceTests
    {
        [Fact]
        public void GivenEmptyStringPlateNumber_WhenPlateDetailsAreRequested_ThenArgumentExceptionIsThrown()
        {
            // Arrange
            var plateLookupService = new PlateLookupService();
            
            // Act 
            Action action = () => plateLookupService.GetPlateDetails("");

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void GivenValidPlateNumber_WhenPlateDetailsAreRequested_ThenBasicDetailsAreReturned()
        {
            // Arrange
            var plateLookupService = new PlateLookupService();
            
            // Act 
            var plateDetails = plateLookupService.GetPlateDetails("ABC123");

            // Assert
            Assert.Equal("", plateDetails);
        }


    }
}
