using System;
using System.Threading.Tasks;
using Refit;

namespace NswLicensePlateLookup.Services
{
    public class PlateLookupService
    {
        public string GetPlateDetails(string plateNumber)
        {
            if (plateNumber == string.Empty)
                throw new ArgumentException();

            GetRequestToken();

            return "";
        }

        private string GetRequestToken()
        {
            var serviceApi = RestService.For<IPlateLookupServiceApi>("https://my.service.nsw.gov.au");

            return "";
        }
    }

    public interface IPlateLookupServiceApi
    {
        [Post("/MyServiceNSW/apexremote")]
        Task<Object> SendServiceNswRequest();
    }
}
