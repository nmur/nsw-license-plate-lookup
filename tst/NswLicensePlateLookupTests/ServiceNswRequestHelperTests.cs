using System;
using Xunit;
using NswLicensePlateLookup.Services;
using NswLicensePlateLookup.Interfaces;
using System.Threading.Tasks;
using FakeItEasy;
using NswLicensePlateLookup.Models;
using System.Collections.Generic;

namespace NswLicensePlateLookupTests
{
    public class ServiceNswRequestHelperTests
    {
        private const string ValidToken = "411a6958-3bc1-45a5-ab38-5b57aff75d75";
        
        private IServiceNswRequestApi _fakeServiceNswRequestApi;

        private IServiceNswRequestHelper _serviceNswRequestHelper;

        public ServiceNswRequestHelperTests()
        {
            _fakeServiceNswRequestApi = A.Fake<IServiceNswRequestApi>();
            _serviceNswRequestHelper = new ServiceNswRequestHelper(_fakeServiceNswRequestApi);
        }

        [Fact]
        public async Task GivenValidPlateNumber_WhenPlateDetailsAreRequested_ThenBasicDetailsAreReturned()
        {
            // Arrange
            var plateNumber = "RWAGON";
            var expectedPlateDetails = "GOLF";
            A.CallTo(() => _fakeServiceNswRequestApi.SendServiceNswRequest<TokenResult>(A<ServiceNswRequestBody>._)).Returns(GetSuccessfulTransactionTokenResponse());
            A.CallTo(() => _fakeServiceNswRequestApi.SendServiceNswRequest<PlateDetailsResult>(A<ServiceNswRequestBody>._)).Returns(GetSuccessfulPlateDetailsResponse());

            // Act 
            var plateDetails = await _serviceNswRequestHelper.GetPlateDetails(plateNumber);

            // Assert
            Assert.Equal(expectedPlateDetails, plateDetails);
        }

        private async Task<List<ServiceNswResponse<TokenResult>>> GetSuccessfulTransactionTokenResponse()
        {
            return new List<ServiceNswResponse<TokenResult>>
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
                        Token = ValidToken
                    }
                }
            };
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
}
