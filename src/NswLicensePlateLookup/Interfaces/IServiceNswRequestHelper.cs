using System.Threading.Tasks;

namespace NswLicensePlateLookup.Interfaces
{
    public interface IServiceNswRequestHelper
    {
        Task<string> GetPlateDetails(string plateNumber);
    }
}