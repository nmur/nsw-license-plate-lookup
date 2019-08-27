using System;
using Xunit;
using NswLicensePlateLookup.Services;

namespace NswLicensePlateLookupTests
{
    public class PlateLookupServiceTests
    {
        [Fact]
        public void GivenEmptyStringPlateNumber_WhenPlateDetailsAreRequested_ThenErrorIsReturned()
        {
            // Arrange
            var plateLookupService = new PlateLookupService();
            
            // Act 
            Action action = () => plateLookupService.GetPlateDetails("");

            // Assert
            Assert.Throws<ArgumentException>(action);
        }
    }
}
