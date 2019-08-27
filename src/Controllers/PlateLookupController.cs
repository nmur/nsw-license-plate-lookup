using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NswLicensePlateLookup.Controllers
{
    [ApiController]
    public class PlateLookupController : ControllerBase
    {
        [HttpGet]
        [Route("api/plate/{plateNumber}")]
        public ActionResult<string> Get(string plateNumber)
        {
            return plateNumber;
        }
    }
}
