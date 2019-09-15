using System;
using System.Threading.Tasks;
using NswLicensePlateLookup.Interfaces;

namespace NswLicensePlateLookup.Services
{
    public class PlateLookupService : IPlateLookupService
    {
        private IServiceNswRequestHelper _serviceNswRequestHelper;

        public PlateLookupService(IServiceNswRequestHelper serviceNswRequestHelper)
        {
            _serviceNswRequestHelper = serviceNswRequestHelper;
        }

        public async Task<string> GetPlateDetails(string plateNumber)
        {
            if (plateNumber == string.Empty)
                throw new ArgumentException();

            return await _serviceNswRequestHelper.GetPlateDetails(plateNumber);
        }
    }
}
