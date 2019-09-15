using System.Collections.Generic;
using System.Threading.Tasks;
using NswLicensePlateLookup.Interfaces;
using NswLicensePlateLookup.Models;
using Refit;

namespace NswLicensePlateLookup.Services
{
    public class ServiceNswRequestHelper : IServiceNswRequestHelper
    {
        private IServiceNswRequestApi _serviceNswRequestApi = RestService.For<IServiceNswRequestApi>("https://my.service.nsw.gov.au");

        public async Task<string> GetPlateDetails(string plateNumber)
        {
            var token = await GetTransactionToken();

            var plateDetailsResponse = await _serviceNswRequestApi.SendServiceNswRequest<PlateDetailsResult>(GetPlateDetailsRequestBody(token, plateNumber));
            return GetPlateDetailsFromResponse(plateDetailsResponse);
        }

        private static ServiceNswRequestBody GetPlateDetailsRequestBody(string token, string plateNumber)
        {
            return new ServiceNswRequestBody
            {
                Method = "postVehicleListForFreeRegoCheck",
                Data = new List<string>{"{\"transactionToken\":\"" + token + "\",\"plateNumber\":\"" + plateNumber + "\"}"}
            };
        }
        
        private string GetPlateDetailsFromResponse(List<ServiceNswResponse<PlateDetailsResult>> plateDetailsResponse) => plateDetailsResponse[0].Result.PlateDetails.Vehicle.Model;

        private async Task<string> GetTransactionToken()
        {
            var tokenResponse = await _serviceNswRequestApi.SendServiceNswRequest<TokenResult>(GetTransactionTokenRequestBody());
            return GetTokenFromResponse(tokenResponse);
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