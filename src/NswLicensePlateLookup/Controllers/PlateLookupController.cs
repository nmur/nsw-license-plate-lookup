using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace NswLicensePlateLookup.Controllers
{
    [ApiController]
    public class PlateLookupController : ControllerBase
    {
        [HttpGet]
        [Route("api/plate/{plateNumber}")]
        public ActionResult<string> GetPlateNumber(string plateNumber)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
