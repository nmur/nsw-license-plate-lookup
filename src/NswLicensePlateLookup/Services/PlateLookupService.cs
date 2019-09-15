using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NswLicensePlateLookup.Models;
using Refit;

namespace NswLicensePlateLookup.Services
{
    public class PlateLookupService
    {
        public async Task<string> GetPlateDetails(string plateNumber)
        {
            if (plateNumber == string.Empty)
                throw new ArgumentException();

            return await SendPlateRequest(plateNumber);
        }

        private async Task<string> SendPlateRequest(string plateNumber)
        {
            var serviceApi = RestService.For<IPlateLookupServiceApi>("https://my.service.nsw.gov.au");

            // get token first
            var serviceNswRequestBody = new ServiceNswRequestBody();
            serviceNswRequestBody.Method = "createRMSTransaction";
            serviceNswRequestBody.Data = new List<string>{"{\"ipAddress\":\"164d9d47a20e4fe26aed22c7cd88e832ee012389c233fc756b3c98f53f5381e6\",\"transactionName\":\"FREEREGCHK\",\"outletNumber\":\"\"}"};
            var tokenResponseList = await serviceApi.SendServiceNswRequest(serviceNswRequestBody);
            var tokenResponse = tokenResponseList[0];
            var token = tokenResponse.Result.StatusObject;

            // use token to request plate details
            serviceNswRequestBody.Method = "postVehicleListForFreeRegoCheck";
            serviceNswRequestBody.Data = new List<string>{"{\"transactionToken\":\"" + token + "\",\"plateNumber\":\"" + plateNumber + "\"}"};
            var plateDetailsResponseList = await serviceApi.SendServiceNswRequest2(serviceNswRequestBody);
            var plateDetailsResponse = plateDetailsResponseList[0];
            
            return plateDetailsResponse.Result.StatusObject.Vehicle.Model;
        }
    }

    public interface IPlateLookupServiceApi
    {
        [Headers("Content-Type: application/json", "origin: https://my.service.nsw.gov.au", "referer: https://my.service.nsw.gov.au/MyServiceNSW/index")]
        [Post("/MyServiceNSW/apexremote")]
        Task<List<TokenResponse>> SendServiceNswRequest(ServiceNswRequestBody body);
        
        //lots of duplicated code here!!
        [Headers("Content-Type: application/json", "origin: https://my.service.nsw.gov.au", "referer: https://my.service.nsw.gov.au/MyServiceNSW/index")]
        [Post("/MyServiceNSW/apexremote")]
        Task<List<PlateDetailsResponse>> SendServiceNswRequest2(ServiceNswRequestBody body);
    }
}
