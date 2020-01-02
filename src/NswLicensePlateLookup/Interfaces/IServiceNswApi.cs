using System.Collections.Generic;
using System.Threading.Tasks;
using NswLicensePlateLookup.Models;
using Refit;

namespace NswLicensePlateLookup.Interfaces
{
    public interface IServiceNswApi
    {
        [Headers("Content-Type: application/json", "origin: https://my.service.nsw.gov.au", "referer: https://my.service.nsw.gov.au/MyServiceNSW/index")]
        [Post("/MyServiceNSW/apexremote")]
        Task<List<ServiceNswResponse<ResultType>>> SendServiceNswRequest<ResultType>(ServiceNswRequestBody body);
    }
}