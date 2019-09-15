using Microsoft.AspNetCore.Mvc;
using NswLicensePlateLookup.Interfaces;
using System.Threading.Tasks;

namespace NswLicensePlateLookup.Controllers
{
    [ApiController]
    public class PlateLookupController : ControllerBase
    {
        private IPlateLookupService _plateLookupService;

        public PlateLookupController(IPlateLookupService plateLookupService)
        {
            _plateLookupService = plateLookupService;
        }

        [HttpGet]
        [Route("api/plate/{plateNumber}")]
        public async Task<ActionResult<string>> GetPlateDetails(string plateNumber)
        {
            var plateDetails = await _plateLookupService.GetPlateDetails(plateNumber);

            return Ok(plateDetails);
        }
    }
}
