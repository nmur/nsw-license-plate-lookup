using Xunit;
using NswLicensePlateLookup.Services;
using NswLicensePlateLookup.Interfaces;
using System.Threading.Tasks;
using FakeItEasy;
using NswLicensePlateLookup.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace NswLicensePlateLookupTests
{
  public class ServiceNswTransactionTokenHelperTests
    {
        private const string ValidToken = "411a6958-3bc1-45a5-ab38-5b57aff75d75";

        private const string ValidToken2 = "df1f30b5-2d64-4a50-92d0-cdb5c96b0413";
        
        private readonly ILogger<ServiceNswTransactionTokenHelper> _fakeLogger;

        private IServiceNswApi _fakeServiceNswApi;

        private IMemoryCache _fakeMemoryCache;

        private IServiceNswTransactionTokenHelper _serviceNswTransactionTokenHelper;

        public ServiceNswTransactionTokenHelperTests()
        {
            _fakeLogger = A.Fake<ILogger<ServiceNswTransactionTokenHelper>>();
            _fakeServiceNswApi = A.Fake<IServiceNswApi>();
            _fakeMemoryCache = new MemoryCache(new MemoryCacheOptions());
            _serviceNswTransactionTokenHelper = new ServiceNswTransactionTokenHelper(_fakeLogger, _fakeServiceNswApi, _fakeMemoryCache);
        }

        [Fact]
        public async Task GivenValidPlateNumber_WhenTransactionTokenAreRequested_ThenValidTransactionTokenIsReturned()
        {
            // Arrange
            A.CallTo(() => _fakeServiceNswApi.SendServiceNswRequest<TokenResult>(A<ServiceNswRequestBody>._)).Returns(GetSuccessfulTransactionTokenResponse());

            // Act 
            var token = await _serviceNswTransactionTokenHelper.GetTransactionToken();

            // Assert
            Assert.Equal(ValidToken, token);
        }

        [Fact]
        public async Task GivenValidPlateNumber_WhenTransactionTokenAreRequestedTwice_ThenTransationTokenIsGeneratedOnceOnly()
        {
            // Arrange
            A.CallTo(() => _fakeServiceNswApi.SendServiceNswRequest<TokenResult>(A<ServiceNswRequestBody>._)).Returns(GetSuccessfulTransactionTokenResponse());

            // Act 
            await _serviceNswTransactionTokenHelper.GetTransactionToken();
            await _serviceNswTransactionTokenHelper.GetTransactionToken();

            // Assert
            A.CallTo(() => _fakeServiceNswApi.SendServiceNswRequest<TokenResult>(A<ServiceNswRequestBody>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GivenValidPlateNumber_WhenTransactionTokenAreRequestedTwiceButCacheIsBypassed_ThenTransationTokenIsGeneratedTwice()
        {
            // Arrange
            A.CallTo(() => _fakeServiceNswApi.SendServiceNswRequest<TokenResult>(A<ServiceNswRequestBody>._)).ReturnsNextFromSequence(GetSuccessfulTransactionTokenResponse2());
            A.CallTo(() => _fakeServiceNswApi.SendServiceNswRequest<TokenResult>(A<ServiceNswRequestBody>._)).ReturnsNextFromSequence(GetSuccessfulTransactionTokenResponse());

            // Act 
            var token1 = await _serviceNswTransactionTokenHelper.GetTransactionToken();
            _serviceNswTransactionTokenHelper.ClearTransactionTokenCache();
            var token2 = await _serviceNswTransactionTokenHelper.GetTransactionToken();

            // Assert
            Assert.Equal(ValidToken, token1);
            Assert.Equal(ValidToken2, token2);
        }

        private List<ServiceNswResponse<TokenResult>> GetSuccessfulTransactionTokenResponse()
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

        private List<ServiceNswResponse<TokenResult>> GetSuccessfulTransactionTokenResponse2()
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
                        Token = ValidToken2
                    }
                }
            };
        }
    }
}
