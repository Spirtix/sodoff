using Microsoft.AspNetCore.Mvc;
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
}
