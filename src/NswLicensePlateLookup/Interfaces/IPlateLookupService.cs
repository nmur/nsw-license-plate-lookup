using System.Threading.Tasks;

namespace NswLicensePlateLookup.Interfaces
{
    public interface IPlateLookupService
    {
        Task<string> GetPlateDetails(string plateNumber);
    }
}