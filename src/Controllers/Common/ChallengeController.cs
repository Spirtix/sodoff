using Microsoft.AspNetCore.Mvc;
using sodoff.Schema;

namespace sodoff.Controllers.Common;
public class ChallengeController : Controller {

    [HttpPost]
    [Produces("application/xml")]
    [Route("ContentWebService.asmx/ChallengeWebService.asmx/GetActiveChallenges")]
    public IActionResult GetActiveChallenges() {
        // TODO: this is a placeholder
        return Ok(new ChallengeInfo[0]);
    }
}
