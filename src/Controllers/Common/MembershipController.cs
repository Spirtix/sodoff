﻿using Microsoft.AspNetCore.Mvc;
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
    [Route("MembershipWebService.asmx/GetChildList")]
    public IActionResult GetChildList([FromForm] string apiToken) {
        User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
        if (user is null)
            // TODO: what response for not logged in?
            return null;

        if (user.Vikings.Count <= 0)
            return null;

        ChildList profiles = new ChildList();
        profiles.strings = user.Vikings.Select(viking => viking.Id + ", " + viking.Name).ToArray();

        return Ok(profiles);
    }
}
