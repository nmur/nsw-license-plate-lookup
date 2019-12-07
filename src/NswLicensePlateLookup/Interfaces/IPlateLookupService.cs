using System.Threading.Tasks;
using NswLicensePlateLookup.Models;

namespace NswLicensePlateLookup.Interfaces
{
    public interface IPlateLookupService
    {
        Task<PlateDetails> GetPlateDetails(string plateNumber);
    }
}