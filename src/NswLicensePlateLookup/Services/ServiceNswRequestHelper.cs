using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NswLicensePlateLookup.Interfaces;
using NswLicensePlateLookup.Models;

namespace NswLicensePlateLookup.Services
{
    public class ServiceNswRequestHelper : IServiceNswRequestHelper
    {
        private readonly ILogger _logger;
        
        private IServiceNswApi _serviceRequestApi;

        private IServiceNswTransactionTokenHelper _serviceNswTransactionTokenHelper;
        
        private IMemoryCache _cache;

        public ServiceNswRequestHelper(ILogger<ServiceNswRequestHelper> logger, IServiceNswApi serviceNswRequestApi, IServiceNswTransactionTokenHelper serviceNswTransactionTokenHelper, IMemoryCache cache)
        {
            _logger = logger;
            _serviceRequestApi = serviceNswRequestApi;
            _serviceNswTransactionTokenHelper = serviceNswTransactionTokenHelper;
            _cache = cache;
        }

        public async Task<PlateDetails> GetPlateDetails(string plateNumber)
        {
            _logger.LogDebug("Begin GetPlateDetails()");
            var token = await _serviceNswTransactionTokenHelper.GetTransactionToken();

            return await
                _cache.GetOrCreateAsync("plate_" + plateNumber, async entry =>
                {
                    _logger.LogDebug("No cached plate - fetching plate details");  
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
                    var plateDetailsResponse = await _serviceRequestApi.SendServiceNswRequest<PlateDetailsResult>(GetPlateDetailsRequestBody(token, plateNumber));
                    _logger.LogDebug("Plate details fetched");  
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
    }
}