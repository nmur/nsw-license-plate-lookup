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
  public class ServiceNswTransactionTokenHelperTests
    {
        private const string ValidToken = "411a6958-3bc1-45a5-ab38-5b57aff75d75";
        
        private IServiceNswApi _fakeServiceNswApi;

        private IMemoryCache _fakeMemoryCache;

        private IServiceNswTransactionTokenHelper _serviceNswTransactionTokenHelper;

        public ServiceNswTransactionTokenHelperTests()
        {
            _fakeServiceNswApi = A.Fake<IServiceNswApi>();
            _fakeMemoryCache = A.Fake<IMemoryCache>();
            _serviceNswTransactionTokenHelper = new ServiceNswTransactionTokenHelper(_fakeServiceNswApi, _fakeMemoryCache);
        }

        [Fact]
        public async Task GivenValidPlateNumber_WhenPlateDetailsAreRequested_ThenPlateDetailsAreReturned()
        {
            // Arrange
            A.CallTo(() => _fakeServiceNswApi.SendServiceNswRequest<TokenResult>(A<ServiceNswRequestBody>._)).Returns(GetSuccessfulTransactionTokenResponse());

            // Act 
            var token = await _serviceNswTransactionTokenHelper.GetTransactionToken();

            // Assert
            Assert.Equal(ValidToken, token);
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
    }
}
