using System;

namespace NswLicensePlateLookup.Services
{
    public class PlateLookupService
    {
        public void GetPlateDetails(string plateNumber)
        {
            if (plateNumber == string.Empty)
                throw new ArgumentException();
        }
    }
}
