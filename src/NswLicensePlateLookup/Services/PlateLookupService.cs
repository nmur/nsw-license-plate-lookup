using System;
using System.Threading.Tasks;

namespace NswLicensePlateLookup.Services
{
    public class PlateLookupService
    {
        private IServiceNswRequestHelper ServiceNswRequestHelper;

        public PlateLookupService(IServiceNswRequestHelper serviceNswRequestHelper)
        {
            ServiceNswRequestHelper = serviceNswRequestHelper;
        }

        public async Task<string> GetPlateDetails(string plateNumber)
        {
            if (plateNumber == string.Empty)
                throw new ArgumentException();

            return await ServiceNswRequestHelper.GetPlateDetails(plateNumber);
        }
    }
}
