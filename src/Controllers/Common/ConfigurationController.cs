using Microsoft.AspNetCore.Mvc;
using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Controllers.Common;

public class ConfigurationController : Controller {

    [HttpPost]
    //[Produces("application/xml")]
    [Route("ConfigurationWebService.asmx/GetMMOServerInfoWithZone")]
    public IActionResult GetMMOServerInfoWithZone() {
        // TODO: this is a placeholder
        return Ok(XmlUtil.ReadResourceXmlString("mmo"));
    }
}
