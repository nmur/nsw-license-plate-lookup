using System.Threading.Tasks;

namespace NswLicensePlateLookup.Services
{
    public interface IServiceNswTransactionTokenHelper
    {
        Task<string> GetTransactionToken();

        void ClearTransactionTokenCache();
    }
}