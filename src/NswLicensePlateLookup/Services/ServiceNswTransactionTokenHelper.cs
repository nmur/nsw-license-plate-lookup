using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NswLicensePlateLookup.Interfaces;
using NswLicensePlateLookup.Models;

namespace NswLicensePlateLookup.Services
{
    public class ServiceNswTransactionTokenHelper : IServiceNswTransactionTokenHelper
    {
        private const string _tokenCacheKey = "token";

        private readonly ILogger _logger;

        private IServiceNswApi _serviceRequestApi;
        
        private IMemoryCache _cache;

        public ServiceNswTransactionTokenHelper(ILogger<ServiceNswTransactionTokenHelper> logger, IServiceNswApi serviceNswRequestApi, IMemoryCache cache)
        {
            _logger = logger;
            _serviceRequestApi = serviceNswRequestApi;
            _cache = cache;
        }
        
        public async Task<string> GetTransactionToken()
        {
            _logger.LogDebug("Begin GetTransactionToken()");
            return await
                _cache.GetOrCreateAsync(_tokenCacheKey, async entry =>
                {
                    _logger.LogDebug("No cached token - fetching new token");    
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
                    var tokenResponse = await _serviceRequestApi.SendServiceNswRequest<TokenResult>(GetTransactionTokenRequestBody());
                    _logger.LogDebug("New token fetched");   
                    return GetTokenFromResponse(tokenResponse);
                });
        }

        public void ClearTransactionTokenCache()
        {
            _cache.Remove(_tokenCacheKey);
        }

        private string GetTokenFromResponse(List<ServiceNswResponse<TokenResult>> tokenResponse) => tokenResponse[0].Result.Token;

        private static ServiceNswRequestBody GetTransactionTokenRequestBody()
        {
            return new ServiceNswRequestBody
            {
                Method = "createRMSTransaction",
                Data = new List<string> { "{\"ipAddress\":\"164d9d47a20e4fe26aed22c7cd88e832ee012389c233fc756b3c98f53f5381e6\",\"transactionName\":\"FREEREGCHK\",\"outletNumber\":\"\"}" }
            };
        }
    }
}