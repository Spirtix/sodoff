using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Controllers.Common;

public class RegistrationController : Controller {

    private readonly DBContext ctx;

    public RegistrationController(DBContext ctx) {
        this.ctx = ctx;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("v3/RegistrationWebService.asmx/RegisterParent")]
    [DecryptRequest("parentRegistrationData")]
    [EncryptResponse]
    public IActionResult RegisterParent() {
        ParentRegistrationData data = XmlUtil.DeserializeXml<ParentRegistrationData>(Request.Form["parentRegistrationData"]);
        User u = new User {
            Id = Guid.NewGuid().ToString(),
            Username = data.ChildList[0].ChildName,
            Password = new PasswordHasher<object>().HashPassword(null, data.Password),
            Email = data.Email
        };

        // Check if user exists
        if (ctx.Users.Count(e => e.Email == u.Email) > 0 || ctx.Users.Count(e => e.Username== u.Username) > 0) {
            return Ok(new RegistrationResult { Status = MembershipUserStatus.DuplicateEmail });
        }

        ctx.Users.Add(u);
        ctx.SaveChanges();

        ParentLoginInfo pli = new ParentLoginInfo {
            UserName = u.Username,
            ApiToken = Guid.NewGuid().ToString(),
            UserID = u.Id,
            Status = MembershipUserStatus.Success,
            UnAuthorized = false
        };

        var response = new RegistrationResult {
            ParentLoginInfo = pli,
            UserID = u.Id,
            Status = MembershipUserStatus.Success,
            ApiToken = Guid.NewGuid().ToString()
        };

        return Ok(response);
    }
}
