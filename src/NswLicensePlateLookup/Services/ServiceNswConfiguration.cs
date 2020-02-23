using System;
using System.Threading.Tasks;
using NswLicensePlateLookup.Interfaces;
using NswLicensePlateLookup.Models;

namespace NswLicensePlateLookup.Services
{
    public class ServiceNswConfiguration : IServiceNswConfiguration
    {
        public Uri GetBaseAddress()
        {
            return new Uri("https://my.service.nsw.gov.au");
        }
    }
}
