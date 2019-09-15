using System;
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

            await SendPlateRequest();

            return "";
        }

        private async Task<string> SendPlateRequest()
        {
            var serviceApi = RestService.For<IPlateLookupServiceApi>("https://my.service.nsw.gov.au");
            var plateDetailsResponse = await serviceApi.SendServiceNswRequest();
            return plateDetailsResponse.ToString();
        }
    }

    public interface IPlateLookupServiceApi
    {
        [Headers("Content-Type: application/json", "origin: https://my.service.nsw.gov.au", "referer: https://my.service.nsw.gov.au/MyServiceNSW/index")]
        [Post("/MyServiceNSW/apexremote")]
        Task<Object> SendServiceNswRequest(ServiceNswRequestBody body);
    }
}
