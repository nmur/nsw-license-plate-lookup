using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using NswLicensePlateLookup.Interfaces;
using NswLicensePlateLookup.Models;

namespace NswLicensePlateLookup.Services
{
    public class ServiceNswRequestHelper : IServiceNswRequestHelper
    {
        private IServiceNswApi _serviceRequestApi;

        private IServiceNswTransactionTokenHelper _serviceNswTransactionTokenHelper;
        
        private IMemoryCache _cache;

        public ServiceNswRequestHelper(IServiceNswApi serviceNswRequestApi, IServiceNswTransactionTokenHelper serviceNswTransactionTokenHelper, IMemoryCache cache)
        {
            _serviceRequestApi = serviceNswRequestApi;
            _serviceNswTransactionTokenHelper = serviceNswTransactionTokenHelper;
            _cache = cache;
        }

        public async Task<PlateDetails> GetPlateDetails(string plateNumber)
        {
            var token = await _serviceNswTransactionTokenHelper.GetTransactionToken();

            return await
                _cache.GetOrCreateAsync("plate_" + plateNumber, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
                    var plateDetailsResponse = await _serviceRequestApi.SendServiceNswRequest<PlateDetailsResult>(GetPlateDetailsRequestBody(token, plateNumber));
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