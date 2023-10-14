using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;

namespace sodoff.Controllers.Common;
public class MembershipController : Controller {

    private readonly DBContext ctx;
    public MembershipController(DBContext ctx) {
        this.ctx = ctx;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("MembershipWebService.asmx/GetSubscriptionInfo")]
    public IActionResult GetSubscriptionInfo() {
        return Ok(new SubscriptionInfo {
            Recurring = true,
            Status = "Member",
            SubscriptionTypeID = 1,
            SubscriptionEndDate = new DateTime(2033, 6, 13, 16, 17, 18)
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("MembershipWebService.asmx/GetChildList")] // used by old SoD (e.g. 2.9)
    [VikingSession(Mode=VikingSession.Modes.USER, UseLock=false)]
    public IActionResult GetChildList(User user) {
        if (user.Vikings.Count <= 0)
            return Ok();

        ChildList profiles = new ChildList();
        profiles.strings = user.Vikings.Select(viking => viking.Uid + ", " + viking.Name).ToArray();

        return Ok(profiles);
    }
}
