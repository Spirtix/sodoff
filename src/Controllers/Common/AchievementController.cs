using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using sodoff.Model;
using sodoff.Util;

namespace sodoff.Controllers.Common;
public class AchievementController : Controller {

    private readonly DBContext ctx;
    public AchievementController(DBContext ctx) {
        this.ctx = ctx;
    }

    [HttpPost]
    //[Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetPetAchievementsByUserID")]
    public IActionResult GetPetAchievementsByUserID() {
        // TODO, this is a placeholder
        return Ok("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<ArrayOfUserAchievementInfo xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://api.jumpstart.com/\" />");
    }

    [HttpPost]
    //[Produces("application/xml")]
    [Route("AchievementWebService.asmx/GetAllRanks")]
    public IActionResult GetAllRanks() {
        // TODO, this is a placeholder
        return Ok(XmlUtil.ReadResourceXmlString("allranks"));
    }
}
