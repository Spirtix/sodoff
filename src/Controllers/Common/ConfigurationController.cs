using Microsoft.AspNetCore.Mvc;
using sodoff.Schema;

namespace sodoff.Controllers.Common;

public class ConfigurationController : Controller {

    [HttpPost]
    [Produces("application/xml")]
    [Route("ConfigurationWebService.asmx/GetMMOServerInfoWithZone")]
    public IActionResult GetMMOServerInfoWithZone() {
        // TODO: this is a placeholder
        return Ok(new MMOServerInformation[0]);
    }
}
