using Xunit;
using NswLicensePlateLookup.Services;
using NswLicensePlateLookup.Interfaces;
using System.Threading.Tasks;
using FakeItEasy;
using NswLicensePlateLookup.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace NswLicensePlateLookupTests
{
  public class ServiceNswRequestHelperTests
    {
        private const string ValidToken = "411a6958-3bc1-45a5-ab38-5b57aff75d75";
        
        private IServiceNswApi _fakeServiceNswApi;

        private IServiceNswTransactionTokenHelper _fakeServiceNswTransactionTokenHelper;

        private IMemoryCache _fakeMemoryCache;

        private IServiceNswRequestHelper _serviceNswRequestHelper;

        public ServiceNswRequestHelperTests()
        {
            _fakeServiceNswApi = A.Fake<IServiceNswApi>();
            _fakeServiceNswTransactionTokenHelper = A.Fake<IServiceNswTransactionTokenHelper>();
            _fakeMemoryCache = A.Fake<IMemoryCache>();
            _serviceNswRequestHelper = new ServiceNswRequestHelper(_fakeServiceNswApi, _fakeServiceNswTransactionTokenHelper, _fakeMemoryCache);
        }

        [Fact]
        public async Task GivenValidPlateNumber_WhenPlateDetailsAreRequested_ThenPlateDetailsAreReturned()
        {
            // Arrange
            var plateNumber = "RWAGON";
            var expectedPlateDetails = SuccessfulPlateDetailsResponseVehicle;
            A.CallTo(() => _fakeServiceNswTransactionTokenHelper.GetTransactionToken()).Returns(ValidToken);
            A.CallTo(() => _fakeServiceNswApi.SendServiceNswRequest<PlateDetailsResult>(A<ServiceNswRequestBody>._)).Returns(GetSuccessfulPlateDetailsResponse());

            // Act 
            var plateDetails = await _serviceNswRequestHelper.GetPlateDetails(plateNumber);

            // Assert
            Assert.Equal(expectedPlateDetails, plateDetails);
        }

        private async Task<List<ServiceNswResponse<PlateDetailsResult>>> GetSuccessfulPlateDetailsResponse()
        {
            return new List<ServiceNswResponse<PlateDetailsResult>>
            {
                new ServiceNswResponse<PlateDetailsResult>
                {
                    StatusCode = 200,
                    Type = "rpc",
                    Tid = 1,
                    Ref = false,
                    Action = "RMSWrapperCtrl",
                    Method = "postVehicleListForFreeRegoCheck",
                    Result = new PlateDetailsResult
                    {
                        StatusCode = 2000,
                        StatusMessage = "success",
                        PlateDetails = SuccessfulPlateDetailsResponseVehicle
                    }
                }
            };
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
