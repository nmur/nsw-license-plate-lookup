using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using NswLicensePlateLookup.Interfaces;
using NswLicensePlateLookup.Models;
using Refit;

namespace NswLicensePlateLookup.Services
{
    public class ServiceNswRequestHelper : IServiceNswRequestHelper
    {
        private IServiceNswRequestApi _serviceNswRequestApi;
        
        private IMemoryCache _cache;

        public ServiceNswRequestHelper(IServiceNswRequestApi serviceNswRequestApi, IMemoryCache cache)
        {
            _serviceNswRequestApi = serviceNswRequestApi;
            _cache = cache;
        }

        public async Task<PlateDetails> GetPlateDetails(string plateNumber)
        {
            var token = await GetTransactionToken();

            return await
                _cache.GetOrCreateAsync("plate_" + plateNumber, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
                    var plateDetailsResponse = await _serviceNswRequestApi.SendServiceNswRequest<PlateDetailsResult>(GetPlateDetailsRequestBody(token, plateNumber));
                    return GetPlateDetailsFromResponse(plateDetailsResponse);
                });
        }

        private static ServiceNswRequestBody GetPlateDetailsRequestBody(string token, string plateNumber)
        {
            return new ServiceNswRequestBody
            {
                Method = "postVehicleListForFreeRegoCheck",
                Data = new List<string>{"{\"transactionToken\":\"" + token + "\",\"plateNumber\":\"" + plateNumber + "\"}"}
            };
        }
        
        private PlateDetails GetPlateDetailsFromResponse(List<ServiceNswResponse<PlateDetailsResult>> plateDetailsResponse) => plateDetailsResponse[0].Result.PlateDetails;

        private async Task<string> GetTransactionToken()
        {
            return await
                _cache.GetOrCreateAsync("token", async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
                    var tokenResponse = await _serviceNswRequestApi.SendServiceNswRequest<TokenResult>(GetTransactionTokenRequestBody());
                    return GetTokenFromResponse(tokenResponse);
                });
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

    public interface IServiceNswRequestApi
    {
        [Headers("Content-Type: application/json", "origin: https://my.service.nsw.gov.au", "referer: https://my.service.nsw.gov.au/MyServiceNSW/index")]
        [Post("/MyServiceNSW/apexremote")]
        Task<List<ServiceNswResponse<ResultType>>> SendServiceNswRequest<ResultType>(ServiceNswRequestBody body);
    }
}