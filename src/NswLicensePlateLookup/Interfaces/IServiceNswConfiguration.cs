using System;

namespace NswLicensePlateLookup.Interfaces
{
    public interface IServiceNswConfiguration
    {
        Uri GetBaseAddress();
    }
}