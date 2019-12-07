using System.Threading.Tasks;
using NswLicensePlateLookup.Models;

namespace NswLicensePlateLookup.Interfaces
{
    public interface IServiceNswRequestHelper
    {
        Task<PlateDetails> GetPlateDetails(string plateNumber);
    }
}